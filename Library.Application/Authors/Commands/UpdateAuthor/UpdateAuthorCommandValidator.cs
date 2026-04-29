namespace Library.Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
	{
		public UpdateAuthorCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationMessages.AuthorInvalidId);

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.AuthorNameRequired)
				.MaximumLength(200).WithMessage(ValidationMessages.AuthorNameMaxLength);

			RuleFor(x => x.Biography)
				.NotEmpty().WithMessage(ValidationMessages.AuthorBiographyRequired)
				.MaximumLength(2000).WithMessage(ValidationMessages.AuthorBiographyMaxLength);
		}
	}
}
