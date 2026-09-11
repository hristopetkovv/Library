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
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseJson = JsonDocument.Parse(responseString).RootElement;

            return await ProcessResponseAsync(responseJson, client, message, language, cancellationToken, maxDepth: 10);
        }

        private async Task<string> ProcessResponseAsync(JsonElement responseJson, HttpClient client, string originalMessage, string language, CancellationToken cancellationToken, int maxDepth = 5)
        {
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

                var functionCalls = new List<(string Name, JsonElement Args, string CallId, string? ThoughtSignature)>();

                foreach (var part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("functionCall", out var functionCall))
                    {
                        var functionName = functionCall.GetProperty("name").GetString();
                        var args = functionCall.GetProperty("args");
                        var callId = functionCall.TryGetProperty("id", out var id) ? id.GetString() : "call_id";
                        var thoughtSignature = part.TryGetProperty("thoughtSignature", out var ts) ? ts.GetString() : null;

                        functionCalls.Add((functionName!, args, callId!, thoughtSignature));
                    }

                    if (part.TryGetProperty("text", out var textElement))
                    {
                        var text = textElement.GetString();
                        if (!string.IsNullOrEmpty(text))
                            return text;
                    }
                }

                if (functionCalls.Count > 0)
                {
                    return await SendMultipleFunctionResultsAsync(client, originalMessage, language, functionCalls, maxDepth - 1, cancellationToken);
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
                "searchBooksByDescription" => await SearchBooksByDescriptionAsync(args, cancellationToken),
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

        private async Task<string> SearchBooksByDescriptionAsync(JsonElement args, CancellationToken cancellationToken)
        {
            var term = args.TryGetProperty("term", out var t) ? t.GetString() : null;

            if (string.IsNullOrEmpty(term))
                return "Няма намерени книги.";

            var books = await unitOfWork.Books.SearchByDescriptionAsync(term, cancellationToken);

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

        private async Task<string> SendMultipleFunctionResultsAsync(
            HttpClient client,
            string originalMessage,
            string language,
            List<(string Name, JsonElement Args, string CallId, string? ThoughtSignature)> functionCalls,
            int maxDepth,
            CancellationToken cancellationToken)
        {
            var functionResults = new List<(string Name, string CallId, string? ThoughtSignature, JsonElement Args, string Result)>();

            foreach (var (name, args, callId, thoughtSignature) in functionCalls)
            {
                var result = await ExecuteFunctionAsync(name, args, cancellationToken);
                functionResults.Add((name, callId, thoughtSignature, args, result));
            }

            var modelParts = functionResults.Select(fr =>
            {
                if (fr.ThoughtSignature != null)
                    return (object)new { functionCall = new { name = fr.Name, args = fr.Args, id = fr.CallId }, thoughtSignature = fr.ThoughtSignature };
                else
                    return (object)new { functionCall = new { name = fr.Name, args = fr.Args, id = fr.CallId } };
            }).ToArray();

            var responseParts = functionResults.Select(fr => (object)new
            {
                functionResponse = new
                {
                    name = fr.Name,
                    id = fr.CallId,
                    response = new { result = fr.Result }
                }
            }).ToArray();

            var requestBody = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = GetSystemPrompt(language) } }
                },
                contents = new object[]
                {
            new { role = "user", parts = new[] { new { text = originalMessage } } },
            new { role = "model", parts = modelParts },
            new { role = "user", parts = responseParts }
                },
                tools = BuildTools()
            };

            var response = await client.PostAsJsonAsync(
                $"{config.GeminiApiBaseUrl}?key={config.GeminiApiKey}",
                requestBody,
                cancellationToken);

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseJson = JsonDocument.Parse(responseString).RootElement;

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
                        },
                        new
                        {
                            name = "searchBooksByDescription",
                            description = "Търси книги по описание - полезно когато потребителят търси по тема или жанр",
                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    term = new { type = "string", description = "Тема или ключова дума за търсене в описанието" }
                                }
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
            
            SEARCHING RULES:
            1. If user searches by title or author → use searchBooks
            2. If user searches by topic, genre or theme → use searchBooksByDescription
               - ALWAYS translate the topic to English and search with the English term
               - Example: "войни" → search "war", "средновековие" → search "medieval", "любов" → search "love"
               - Also try Bulgarian term simultaneously
            3. If ANY search returns results → USE THEM IMMEDIATELY to answer. Do NOT search again.
            4. If ALL searches return empty → tell the user no books were found in the library, 
               then suggest 3-5 popular books on the topic from your own knowledge.
               Make clear these suggestions are from your knowledge and may not be in the library.
            
            AVAILABILITY RULES:
            - If user asks when a book will be available → first find it with searchBooks, then use getEarliestReturnDate with its ID
            
            IMPORTANT:
            - If ANY search returns results, stop searching and answer immediately
            - Never search again if you already have results
            - Never call the same function with the same parameters more than once
            """;
    }
}
