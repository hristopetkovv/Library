namespace Library.Infrastructure.Services.Helpers
{
    public class LocalFileStorageService(
        IWebHostEnvironment env,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    ) : IFileStorageService
    {
        private const string uploadFolder = "uploads/books";
        private readonly string apiBaseUrl = configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(contentType))
                throw new InvalidOperationException("Unsupported image format.");

            var webRootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploadsPath = Path.Combine(webRootPath, uploadFolder);
            Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(fileName);
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, uniqueName);

            await using var stream = File.Create(filePath);
            await fileStream.CopyToAsync(stream, cancellationToken);

            var request = httpContextAccessor.HttpContext?.Request;

            if (request is null)
                return $"{apiBaseUrl}/{uploadFolder}/{uniqueName}";


            return $"{request.Scheme}://{request.Host}/{uploadFolder}/{uniqueName}";
        }

        public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
            var filePath = Path.Combine(env.WebRootPath, uploadFolder, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}
