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
			// Remove hyphens and spaces
			var cleaned = isbn.Replace("-", "").Replace(" ", "");

			if (!IsValidISBN(cleaned))
				throw new ArgumentException(ValidationMessages.BookISBNInvalidFormat);

			return new ISBN(cleaned);
		}

		private static bool IsValidISBN(string isbn)
		{
			// ISBN-10 or ISBN-13
			return Regex.IsMatch(isbn, @"^\d{10}(\d{3})?$");
		}
	}
}
