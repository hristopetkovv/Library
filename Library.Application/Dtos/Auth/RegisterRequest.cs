namespace Library.Application.Dtos.Auth
{
	public record RegisterRequest(
		string Password,
		string PasswordAgain,
		string Email,
		string FirstName,
		string LastName,
		string? PhoneNumber,
		string? Address
	);
}
