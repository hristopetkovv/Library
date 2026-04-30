namespace Library.Application.Interfaces.Helpers
{
	public interface IFileStorageService
	{
		Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
		Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
	}
}
