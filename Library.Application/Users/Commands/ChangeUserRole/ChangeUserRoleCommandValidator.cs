namespace Library.Application.Users.Commands.ChangeUserRole
{
	public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
	{
		public ChangeUserRoleCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationMessages.UserInvalidId);

			RuleFor(x => x.NewRole)
				.NotNull().WithMessage(ValidationMessages.UserRoleRequired);
		}
	}
}
