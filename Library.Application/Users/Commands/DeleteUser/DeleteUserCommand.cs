namespace Library.Application.Users.Commands.DeleteUser
{
	public record DeleteUserCommand(int Id) : ICommand<Unit>;
}
