namespace Library.Domain.ValueObjects.Users
{
	public record Email
	{
		private Email(string value)
		{
			Value = value;
		}

		public string Value { get; }

		public static Email Create(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email is required");

			if (!IsValidEmail(email))
				throw new ArgumentException("Invalid email format");

			return new Email(email.ToLowerInvariant());
		}

		private static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
		}
	}
}
