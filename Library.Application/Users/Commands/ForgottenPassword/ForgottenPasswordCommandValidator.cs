namespace Library.Application.Users.Commands.ForgottenPassword
{
	public class ForgottenPasswordCommandValidator : AbstractValidator<ForgottenPasswordCommand>
	{
		public ForgottenPasswordCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required")
				.Matches(ValidationRegexes.Email);
		}
	}
}
