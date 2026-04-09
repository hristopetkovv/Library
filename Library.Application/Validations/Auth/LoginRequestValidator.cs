namespace Library.Application.Validations.Auth
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email cannot be empty.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password cannot be empty.");
		}
	}
}
