namespace Library.Application.Users.Dtos
{
	public record UserDetailDto(
		int Id,
		string Email,
		UserRole Role,
		string FirstName,
		string LastName,
		string? Address,
		string? PhoneNumber
	);
}
