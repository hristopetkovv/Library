namespace Library.Contracts.Users
{
	public record ChangeUserRoleRequest(
		UserRole NewRole
	);
}
