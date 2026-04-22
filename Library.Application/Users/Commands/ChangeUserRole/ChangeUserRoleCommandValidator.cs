namespace Library.Application.Users.Commands.ChangeUserRole
{
	public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
	{
		public ChangeUserRoleCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid User ID is required");

			RuleFor(x => x.NewRole)
				.NotNull().WithMessage("New role is required");
		}
	}
}
