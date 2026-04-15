namespace Library.Application.Interfaces.Helpers
{
	public interface IPasswordHasher
	{
		string GenerateHash(string password, string salt);
		string GenerateSalt();
		bool VerifyPassword(string password, string hash, string salt);
	}
}
