namespace Library.Application.Authors.Commands.CreateAuthor
{
	public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
	{
		public CreateAuthorCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.AuthorNameRequired)
				.MaximumLength(200).WithMessage(ValidationMessages.AuthorNameMaxLength);

			RuleFor(x => x.Biography)
				.NotEmpty().WithMessage(ValidationMessages.AuthorBiographyRequired)
				.MaximumLength(2000).WithMessage(ValidationMessages.AuthorBiographyMaxLength);
		}
	}
}
