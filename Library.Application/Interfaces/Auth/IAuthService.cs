namespace Library.Application.Interfaces.Auth
{
	public interface IAuthService
	{
		Task<AuthResponse> Login(LoginRequest request);
		Task Register(RegisterRequest request);
	}
}
