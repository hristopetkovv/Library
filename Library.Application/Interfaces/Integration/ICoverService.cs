namespace Library.Application.Interfaces.Integration
{
    public interface ICoverService
    {
        Task<string?> TryDownloadCoverAsync(string isbn);
    }
}
