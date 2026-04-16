namespace Library.Infrastructure.Services.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int keySize = 32;
        private readonly int iterations = 210000;

		public string HashPassword(string password, out string salt)
		{
			var saltBytes = RandomNumberGenerator.GetBytes(keySize);
			salt = Convert.ToHexString(saltBytes);
			var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes, iterations, HashAlgorithmName.SHA512, keySize);

			return Convert.ToHexString(hash);
		}

		public bool VerifyPassword(string providedPassword, string hashedPassword, string salt)
        {
			if (string.IsNullOrWhiteSpace(hashedPassword)
				|| string.IsNullOrWhiteSpace(salt)
				|| string.IsNullOrWhiteSpace(providedPassword))
			{
				return false;
			}

			var saltBytes = Convert.FromHexString(salt);

			var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(providedPassword, saltBytes, iterations, HashAlgorithmName.SHA512, keySize);

			return hashToCompare.SequenceEqual(Convert.FromHexString(hashedPassword));
		}
    }
}
