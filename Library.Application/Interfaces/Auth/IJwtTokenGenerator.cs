namespace Library.Application.Interfaces.Auth
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(User user);
	}
}
