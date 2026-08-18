namespace Library.Application.Interfaces.Integration
{
    public interface IDescriptionService
    {
        Task<string?> TryGetDescriptionAsync(string isbn);
    }
}
