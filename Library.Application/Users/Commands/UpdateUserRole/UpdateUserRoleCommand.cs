namespace Library.Application.Users.Commands.UpdateUserRole
{
	public record UpdateUserRoleCommand(
		int Id,
		UserRole NewRole
	) : ICommand<Unit>;
}
