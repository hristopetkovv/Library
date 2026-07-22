namespace Library.Infrastructure.Services.Helpers
{
    public class DescriptionService(IHttpClientFactory httpClientFactory, IOptions<ExternalServicesConfiguration> externalServicesOptions) : IDescriptionService
    {
        private readonly ExternalServicesConfiguration externalServicesConfig = externalServicesOptions.Value;

        public async Task<string?> TryGetDescriptionAsync(string isbn)
        {
            try
            {
                var description = await TryGetFromGoogleBooksAsync(isbn);
                if (!string.IsNullOrEmpty(description))
                    return description;

                return await TryGetFromOpenLibraryAsync(isbn);
            }
            catch
            {
                return null;
            }
        }

        private async Task<string?> TryGetFromGoogleBooksAsync(string isbn)
        {
            var client = httpClientFactory.CreateClient("GoogleBooks");
            var response = await client.GetAsync($"{externalServicesConfig.GoogleBooksApiBaseUrl}?q=isbn:{isbn}&key={externalServicesConfig.GoogleBooksApiKey}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var items = doc.RootElement.GetProperty("items");
            if (items.GetArrayLength() == 0)
                return null;

            var volumeInfo = items[0].GetProperty("volumeInfo");

            if (volumeInfo.TryGetProperty("description", out var descriptionElement))
                return descriptionElement.GetString();

            return null;
        }

        private async Task<string?> TryGetFromOpenLibraryAsync(string isbn)
        {
            try
            {
                var client = httpClientFactory.CreateClient("OpenLibrary");

                var bookResponse = await client.GetAsync($"https://openlibrary.org/isbn/{isbn}.json");
                if (!bookResponse.IsSuccessStatusCode) return null;

                var bookJson = await bookResponse.Content.ReadAsStringAsync();
                var bookDoc = JsonDocument.Parse(bookJson);

                if (!bookDoc.RootElement.TryGetProperty("works", out var works)) return null;

                var workKey = works[0].GetProperty("key").GetString();

                var workResponse = await client.GetAsync($"https://openlibrary.org{workKey}.json");
                if (!workResponse.IsSuccessStatusCode) return null;

                var workJson = await workResponse.Content.ReadAsStringAsync();
                var workDoc = JsonDocument.Parse(workJson);

                if (!workDoc.RootElement.TryGetProperty("description", out var desc)) return null;

                return desc.ValueKind == JsonValueKind.String
                    ? desc.GetString()
                    : desc.TryGetProperty("value", out var val) ? val.GetString() : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
