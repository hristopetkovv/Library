namespace Library.Infrastructure.Services.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int keySize = 32;
        private readonly int iterations = 5000;

        public (string Hash, string Salt) HashPassword(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(keySize);
            var salt = Convert.ToHexString(saltBytes);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes, iterations, HashAlgorithmName.SHA512, keySize);

            return (Convert.ToHexString(hash), salt);
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
