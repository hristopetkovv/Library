namespace Library.Application.Users.Commands.DeleteUser
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
		public DeleteUserCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationMessages.UserInvalidId);
		}
	}
}
