namespace Library.Application.Interfaces.Auth
{
	public interface IAuthService
	{
		Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
		Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
	}
}
