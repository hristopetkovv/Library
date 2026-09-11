namespace Library.Application.Interfaces.Auth
{
	public interface IAuthService : IScopedService
    {
		Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
		Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
	}
}
