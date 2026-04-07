namespace Library.Application.Users.Dtos
{
	public record UserDto(
		int Id,
		string Email,
		UserRole Role,
		string FirstName,
		string LastName,
		string? Address,
		string? PhoneNumber
	);
}
