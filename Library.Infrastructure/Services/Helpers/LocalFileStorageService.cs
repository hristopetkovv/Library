namespace Library.Infrastructure.Services.Helpers
{
	public class LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : IFileStorageService
	{
		private const string UploadFolder = "uploads/books";

		public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
		{
			var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
			if (!allowedTypes.Contains(contentType))
				throw new InvalidOperationException("Unsupported image format.");

			var uploadsPath = Path.Combine(env.WebRootPath, UploadFolder);
			Directory.CreateDirectory(uploadsPath);

			var extension = Path.GetExtension(fileName);
			var uniqueName = $"{Guid.NewGuid()}{extension}";
			var filePath = Path.Combine(uploadsPath, uniqueName);

			await using var stream = File.Create(filePath);
			await fileStream.CopyToAsync(stream, cancellationToken);

			var request = httpContextAccessor.HttpContext!.Request;
			return $"{request.Scheme}://{request.Host}/{UploadFolder}/{uniqueName}";
		}

		public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
		{
			var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
			var filePath = Path.Combine(env.WebRootPath, UploadFolder, fileName);

			if (File.Exists(filePath))
				File.Delete(filePath);

			return Task.CompletedTask;
		}
	}
}
