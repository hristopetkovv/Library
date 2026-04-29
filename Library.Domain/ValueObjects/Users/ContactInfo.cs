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
				throw new DomainException(ValidationMessages.UserAddressRequired);

			if (string.IsNullOrWhiteSpace(phoneNumber))
				throw new DomainException(ValidationMessages.UserPhoneNumberRequired);

			return new ContactInfo(address, phoneNumber);
		}
	}
}
