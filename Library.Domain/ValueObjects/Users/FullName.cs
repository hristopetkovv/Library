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
				throw new DomainException("First name is required");

			if (string.IsNullOrWhiteSpace(lastName))
				throw new DomainException("Last name is required");

			return new FullName(firstName, lastName);
		}
	}
}
