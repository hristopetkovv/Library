namespace Library.Infrastructure.Services.Auth
{
	public class PasswordService : IPasswordService
	{
		private const int keySize = 32;
		private const int iterations = 5000;

		public string HashPassword(string password, out string salt)
		{
			var saltBytes = RandomNumberGenerator.GetBytes(keySize);
			salt = Convert.ToHexString(saltBytes);
			var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes, iterations, HashAlgorithmName.SHA512, keySize);

			return Convert.ToHexString(hash);
		}
	}
}
