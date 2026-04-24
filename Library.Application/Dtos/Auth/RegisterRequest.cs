namespace Library.Application.Dtos.Auth
{
	public record RegisterRequest(
		string Email,
		string Password,
		string PasswordAgain,
		string FirstName,
		string LastName,
		string? PhoneNumber,
		string? Address
	);
}
