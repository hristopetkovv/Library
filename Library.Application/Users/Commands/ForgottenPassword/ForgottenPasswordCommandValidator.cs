namespace Library.Application.Users.Commands.ForgottenPassword
{
	public class ForgottenPasswordCommandValidator : AbstractValidator<ForgottenPasswordCommand>
	{
		public ForgottenPasswordCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(ValidationMessages.UserEmailRequired)
				.Matches(ValidationRegexes.Email);
		}
	}
}
