namespace Library.Contracts.Users
{
	public record UpdateUserRequest(
		string Email,
		string FirstName,
		string LastName,
		string? Address,
		string? PhoneNumber
	);
}
