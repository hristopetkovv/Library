namespace Library.Application.Users.Dtos
{
	public record UserLoginInfoDto(
		int Id,
		string FirstName,
		string LastName,
		string Email,
		UserRole Role
	);
}
