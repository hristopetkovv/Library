namespace Library.Infrastructure.Services.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int keySize = 32;
        private readonly int iterations = 5000;

        public string GenerateHash(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512, keySize);

            return Convert.ToHexString(hash);
        }

        public string GenerateSalt()
        {
            var saltBytes = RandomNumberGenerator.GetBytes(keySize);

            return Convert.ToHexString(saltBytes);
		}

		public bool VerifyPassword(string password, string hash, string salt)
        {
			if (string.IsNullOrWhiteSpace(hash)
				|| string.IsNullOrWhiteSpace(salt)
				|| string.IsNullOrWhiteSpace(password))
			{
				return false;
			}

			var saltBytes = Convert.FromHexString(salt);
			var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, iterations, HashAlgorithmName.SHA512, keySize);

			return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
		}
    }
}
