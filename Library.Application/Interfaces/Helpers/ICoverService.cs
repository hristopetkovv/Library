namespace Library.Application.Interfaces.Helpers
{
    public interface ICoverService
    {
        Task<string?> TryDownloadCoverAsync(string isbn);
    }
}
