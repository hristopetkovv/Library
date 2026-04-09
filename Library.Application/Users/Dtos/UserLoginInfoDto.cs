namespace Library.Application.Users.Dtos
{
	public record UserLoginInfoDto(
		string FirstName,
		string LastName,
		string Email,
		UserRole Role
	);
}
