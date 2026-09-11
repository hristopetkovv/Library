namespace Library.Application.Interfaces.Integration
{
    public interface IDescriptionService : IScopedService
    {
        Task<string?> TryGetDescriptionAsync(string isbn);
    }
}
