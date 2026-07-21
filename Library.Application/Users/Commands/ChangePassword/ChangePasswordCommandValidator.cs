namespace Library.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired)
                .Matches(ValidationRegexes.Password).WithMessage(ValidationMessages.UserPasswordInvalidRequirements);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired)
                .Matches(ValidationRegexes.Password).WithMessage(ValidationMessages.UserPasswordInvalidRequirements);
        }
    }
}
