namespace Library.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required")
				.Matches(ValidationRegexes.Email);

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("First name name is required")
				.MaximumLength(100).WithMessage("First name cannot exceed 50 characters");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last name name is required")
				.MaximumLength(100).WithMessage("Last name cannot exceed 50 characters");

			RuleFor(x => x.Address)
				.MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
				.When(x => !string.IsNullOrWhiteSpace(x.Address));

			RuleFor(x => x.PhoneNumber)
				.MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
				.When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
		}
	}
}
