namespace Library.Application.Users.Commands.ChangeUserRole
{
	public record ChangeUserRoleCommand(
		int Id,
		UserRole NewRole
	) : ICommand<Unit>;
}
