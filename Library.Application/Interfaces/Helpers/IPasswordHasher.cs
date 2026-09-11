namespace Library.Application.Interfaces.Helpers
{
	public interface IPasswordHasher : ISingletonService
    {
		string HashPassword(string password, out string salt);
		bool VerifyPassword(string providedPassword, string hashedPassword, string salt);
	}
}
