namespace Library.Application.Users.Commands.UpdateUserRole
{
	public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
	{
		public UpdateUserRoleCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid User ID is required");

			RuleFor(x => x.NewRole)
				.NotNull().WithMessage("New role is required");
		}
	}
}
