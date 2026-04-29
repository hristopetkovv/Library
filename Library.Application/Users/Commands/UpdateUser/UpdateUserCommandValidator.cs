namespace Library.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(ValidationMessages.UserEmailRequired)
				.Matches(ValidationRegexes.Email);

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage(ValidationMessages.UserFirstNameRequired)
				.MaximumLength(100).WithMessage(ValidationMessages.UserFirstNameMaxLength);

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage(ValidationMessages.UserLastNameRequired)
				.MaximumLength(100).WithMessage(ValidationMessages.UserLastNameMaxLength);

			RuleFor(x => x.Address)
				.MaximumLength(500).WithMessage(ValidationMessages.UserAddressMaxLength)
				.When(x => !string.IsNullOrWhiteSpace(x.Address));

			RuleFor(x => x.PhoneNumber)
				.MaximumLength(20).WithMessage(ValidationMessages.UserPhoneNumberMaxLength)
				.When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
		}
	}
}
