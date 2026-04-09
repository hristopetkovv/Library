namespace Library.Application.Users.Dtos
{
	public record UserListDto(
		int Id,
		string Email,
		string FirstName,
		string LastName
	);
}
