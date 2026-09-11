namespace Library.Application.Interfaces.Auth
{
	public interface IJwtTokenGenerator : ISingletonService
    {
		string GenerateToken(User user);
	}
}
