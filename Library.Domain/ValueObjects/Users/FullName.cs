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
				throw new DomainException(ValidationMessages.UserFirstNameRequired);

			if (string.IsNullOrWhiteSpace(lastName))
				throw new DomainException(ValidationMessages.UserLastNameRequired);

			return new FullName(firstName, lastName);
		}
	}
}
