namespace Library.Application.Validations.Auth
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(ValidationMessages.UserEmailRequired);

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired);
		}
	}
}
