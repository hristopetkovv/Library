namespace Library.Infrastructure.Services.Helpers
{
    public class CoverService(IFileStorageService fileStorageService, IHttpClientFactory httpClientFactory) : ICoverService
    {
        public async Task<string?> TryDownloadCoverAsync(string isbn)
        {
            try
            {
                var client = httpClientFactory.CreateClient("OpenLibrary");
                var url = $"https://covers.openlibrary.org/b/isbn/{isbn}-L.jpg";

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                // OpenLibrary връща 1x1 gif ако няма снимка
                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (contentType is null || !contentType.StartsWith("image/jpeg"))
                    return null;

                var contentLength = response.Content.Headers.ContentLength;
                if (contentLength is < 1000) // под 1KB = placeholder gif
                    return null;

                await using var stream = await response.Content.ReadAsStreamAsync();

                var savedUrl = await fileStorageService.SaveFileAsync(stream, $"{isbn}.jpg", "image/jpeg");

                return savedUrl;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
