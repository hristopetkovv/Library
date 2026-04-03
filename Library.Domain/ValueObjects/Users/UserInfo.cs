namespace Library.Domain.ValueObjects.Users
{
	public record FullName
	{
		private FullName(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}

		public string FirstName { get; }
		public string LastName { get; }
		public string FullNameString => $"{FirstName} {LastName}";

		public static FullName Create(string firstName, string lastName)
		{
			if (string.IsNullOrWhiteSpace(firstName))
				throw new ArgumentException("First name is required", nameof(firstName));
			if (string.IsNullOrWhiteSpace(lastName))
				throw new ArgumentException("Last name is required", nameof(lastName));

			return new FullName(firstName, lastName);
		}
	}
}
