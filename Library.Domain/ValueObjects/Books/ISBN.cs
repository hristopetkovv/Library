namespace Library.Domain.ValueObjects.Books
{
	public record ISBN
	{
		private ISBN(string value)
		{
			Value = value;
		}

		public string Value { get; }

		public static ISBN Create(string isbn)
		{
			if (string.IsNullOrWhiteSpace(isbn))
				throw new ArgumentException("ISBN is required");

			// Remove hyphens and spaces
			var cleaned = isbn.Replace("-", "").Replace(" ", "");

			if (!IsValidISBN(cleaned))
				throw new ArgumentException("Invalid ISBN format");

			return new ISBN(cleaned);
		}

		private static bool IsValidISBN(string isbn)
		{
			// ISBN-10 or ISBN-13
			return Regex.IsMatch(isbn, @"^\d{10}(\d{3})?$");
		}
	}
}
