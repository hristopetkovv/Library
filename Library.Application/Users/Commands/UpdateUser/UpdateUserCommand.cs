namespace Library.Application.Users.Commands.UpdateUser
{
	public record UpdateUserCommand(
		string Email,
		string FirstName,
		string LastName,
		string? Address,
		string? PhoneNumber
	) : ICommand<UserDetailDto>;
}
