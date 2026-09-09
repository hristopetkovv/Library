namespace Library.Application.Interfaces.Integration
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(string message, string language, CancellationToken cancellationToken = default);
    }
}
