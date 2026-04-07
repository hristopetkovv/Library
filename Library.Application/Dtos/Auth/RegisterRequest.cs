namespace Library.Application.Dtos.Auth
{
	public record RegisterRequest(
		string Password,
		string Email,
		string FirstName,
		string LastName,
		string? PhoneNumber,
		string? Address
	);
}
