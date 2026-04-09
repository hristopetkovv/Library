namespace Library.Application.Users.Commands.UpdateUser
{
	public record UpdateUserCommand(
		int Id,
		string Email,
		string FirstName,
		string LastName,
		string? Address,
		string? PhoneNumber
	) : ICommand<Unit>;
}
