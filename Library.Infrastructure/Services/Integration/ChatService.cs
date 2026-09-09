namespace Library.Infrastructure.Services.Integration
{
    public class ChatService(
        IHttpClientFactory httpClientFactory,
        IOptions<ExternalServicesConfiguration> externalServicesOptions,
        IUnitOfWork unitOfWork
        ) : IChatService
    {
        private readonly ExternalServicesConfiguration config = externalServicesOptions.Value;

        public async Task<string> SendMessageAsync(string message, string language, CancellationToken cancellationToken = default)
        {
            var client = httpClientFactory.CreateClient("Gemini");

            var tools = BuildTools();
            var requestBody = BuildRequest(message, language, tools);

            var response = await client.PostAsJsonAsync($"{config.GeminiApiBaseUrl}?key={config.GeminiApiKey}", requestBody, cancellationToken);
            var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);

            return await ProcessResponseAsync(responseJson, client, message, language, cancellationToken, maxDepth: 10);
        }

        private async Task<string> ProcessResponseAsync(JsonElement responseJson, HttpClient client, string originalMessage, string language, CancellationToken cancellationToken, int maxDepth = 5)
        {
            Console.WriteLine($"ProcessResponseAsync depth: {maxDepth}");

            if (maxDepth <= 0)
                return "Не мога да намеря отговор на този въпрос.";

            try
            {
                if (!responseJson.TryGetProperty("candidates", out var candidates) ||
                    candidates.GetArrayLength() == 0)
                    return "Не мога да отговоря на този въпрос.";

                var candidate = candidates[0];

                if (!candidate.TryGetProperty("content", out var content))
                    return "Не мога да отговоря на този въпрос.";

                if (!content.TryGetProperty("parts", out var parts))
                    return "Не мога да отговоря на този въпрос.";

                foreach (var part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("functionCall", out var functionCall))
                    {
                        var functionName = functionCall.GetProperty("name").GetString();
                        var args = functionCall.GetProperty("args");
                        var callId = functionCall.TryGetProperty("id", out var id) ? id.GetString() : "call_id";
                        var thoughtSignature = part.TryGetProperty("thoughtSignature", out var ts) ? ts.GetString() : null;

                        var functionResult = await ExecuteFunctionAsync(functionName!, args, cancellationToken);

                        return await SendFunctionResultAsync(client, originalMessage, language, functionName!, callId!, functionResult, thoughtSignature, cancellationToken, maxDepth - 1);
                    }

                    if (part.TryGetProperty("text", out var textElement))
                    {
                        var text = textElement.GetString();
                        if (!string.IsNullOrEmpty(text))
                            return text;
                    }
                }

                return "Не мога да отговоря на този въпрос.";
            }
            catch (Exception)
            {
                return "Не мога да отговоря на този въпрос.";
            }
        }

        private async Task<string> ExecuteFunctionAsync(string functionName, JsonElement args, CancellationToken cancellationToken)
        {
            return functionName switch
            {
                "searchBooks" => await SearchBooksAsync(args, cancellationToken),
                "getEarliestReturnDate" => await GetEarliestReturnDateAsync(args, cancellationToken),
                _ => "Функцията не е намерена."
            };
        }

        private async Task<string> SearchBooksAsync(JsonElement args, CancellationToken cancellationToken)
        {
            var term = args.TryGetProperty("term", out var t) ? t.GetString() : null;

            var filter = new SearchBooksFilterDto(term, null, null, null, null);
            var books = await unitOfWork.Books.GetAllFilteredAsync(filter.Predicate(), cancellationToken, b => b.Author);

            if (books.Count == 0)
                return "Няма намерени книги.";

            var result = books.Select(b => new
            {
                b.Id,
                b.Title,
                Author = b.Author.Name,
                b.AvailableCopies,
                b.TotalCopies
            });

            return JsonSerializer.Serialize(result);
        }

        private async Task<string> GetEarliestReturnDateAsync(JsonElement args, CancellationToken cancellationToken)
        {
            var bookId = args.TryGetProperty("bookId", out var bid)
                ? bid.GetInt32()
                : 0;

            var borrowing = await unitOfWork.Borrowings.GetEarliestReturnAsync(bookId, cancellationToken);

            if (borrowing is null)
                return "Няма активни заемания за тази книга.";

            return JsonSerializer.Serialize(new { borrowing.DueDate });
        }

        private async Task<string> SendFunctionResultAsync(HttpClient client, string originalMessage, string language, string functionName, string callId, string functionResult, string? thoughtSignature, CancellationToken cancellationToken, int maxDepth)
        {
            var modelPart = thoughtSignature != null
                ? new object[] { new { functionCall = new { name = functionName, args = new { } }, thoughtSignature } }
                : new object[] { new { functionCall = new { name = functionName, args = new { } } } };

            var requestBody = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = GetSystemPrompt(language) } }
                },
                contents = new object[]
                {
            new { role = "user", parts = new[] { new { text = originalMessage } } },
            new
                {
                    role = "model",
                    parts = new object[]
                    {
                        new { functionCall = new { name = functionName, args = new { }, id = callId }, thoughtSignature }
                    }
                },
            new
            {
                role = "user",
                parts = new[]
                {
                    new
                    {
                        functionResponse = new
                        {
                            name = functionName,
                            id = callId,
                            response = new { result = functionResult }
                        }
                    }
                }
            }
                },
                tools = BuildTools()
            };

            var response = await client.PostAsJsonAsync($"{config.GeminiApiBaseUrl}?key={config.GeminiApiKey}", requestBody, cancellationToken);

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseJson = JsonDocument.Parse(responseString).RootElement;

            var parts = responseJson
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts");

            return await ProcessResponseAsync(responseJson, client, originalMessage, language, cancellationToken, maxDepth);
        }

        private static object BuildTools()
        {
            return new[]
            {
                new
                {
                    function_declarations = new object[]
                    {
                        new
                        {
                            name = "searchBooks",
                            description = "Търси книги в библиотеката по заглавие или автор",
                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    term = new { type = "string", description = "Търсен термин - заглавие, автор или ISBN" }
                                }
                            }
                        },
                        new
                        {
                            name = "getEarliestReturnDate",
                            description = "Връща най-ранната дата на връщане за книга която няма налични копия",
                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    bookId = new { type = "integer", description = "ID на книгата" }
                                },
                                required = new[] { "bookId" }
                            }
                        }
                    }
                }
            };
        }

        private static object BuildRequest(string message, string language, object tools)
        {
            return new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = GetSystemPrompt(language) } }
                },
                contents = new[]
            {
                new { role = "user", parts = new[] { new { text = message } } }
            },
                tools
            };
        }

        private static string GetSystemPrompt(string language) => $"""
            You are a library assistant. Help users find books and check availability.
            {(language == "bg" ? "Отговаряй на български език." : "Reply in English.")}
            
            When searching for books use the searchBooks function.
            Books in the library may have English titles. If the user searches in Bulgarian, 
            also try searching with the English translation.
            
            When a user asks when a book will be available, first find it with searchBooks, 
            then use getEarliestReturnDate with its ID.
            
            IMPORTANT: If searchBooks returns no results or empty list:
            - Do NOT call searchBooks again with the same term
            - Tell the user no books were found matching their criteria
            - Search for popular books related to their topic using searchBooks with English titles
            - For example if user asks for medieval books, search for "Game of Thrones", "Pillars of the Earth", "Ivanhoe" etc.
            - Only suggest books that are actually found in the library via searchBooks
            - If none are found, then suggest titles from your knowledge but make clear they may not be in the library
            
            Never call the same function more than once with the same parameters.
            """;
        }
}
