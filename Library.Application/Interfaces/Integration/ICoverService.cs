namespace Library.Application.Interfaces.Integration
{
    public interface ICoverService : IScopedService
    {
        Task<string?> TryDownloadCoverAsync(string isbn);
    }
}
