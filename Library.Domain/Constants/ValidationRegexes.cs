namespace Library.Domain.Constants
{
	public static class ValidationRegexes
	{
		public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
		public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
	}
}
