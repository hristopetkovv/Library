namespace Library.Application.Interfaces.Helpers
{
    public interface IDescriptionService
    {
        Task<string?> TryGetDescriptionAsync(string isbn);
    }
}
