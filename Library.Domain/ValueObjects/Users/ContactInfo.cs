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
				throw new ArgumentException("Address is required", nameof(address));
			if (string.IsNullOrWhiteSpace(phoneNumber))
				throw new ArgumentException("Phone number is required", nameof(phoneNumber));

			return new ContactInfo(address, phoneNumber);
		}
	}
}
