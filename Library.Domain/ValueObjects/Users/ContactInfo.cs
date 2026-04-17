namespace Library.Domain.ValueObjects.Users
{
	public record ContactInfo
	{
		private ContactInfo(string address, string phoneNumber)
		{
			Address = address;
			PhoneNumber = phoneNumber;
		}

		public string Address { get; }

		public string PhoneNumber { get; }

		public static ContactInfo Create(string address, string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(address))
				throw new DomainException("Address is required");

			if (string.IsNullOrWhiteSpace(phoneNumber))
				throw new DomainException("Phone number is required");

			return new ContactInfo(address, phoneNumber);
		}
	}
}
