namespace Library.Application.Books.Commands.CreateBook
{
	public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
	{
		public CreateBookCommandValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage(ValidationMessages.BookTitleRequired)
				.MaximumLength(300).WithMessage(ValidationMessages.BookTitleMaxLength);

			RuleFor(x => x.AuthorId)
				.GreaterThan(0).WithMessage(ValidationMessages.AuthorInvalidId);

			RuleFor(x => x.PublisherId)
				.GreaterThan(0).WithMessage(ValidationMessages.PublisherInvalidId);

			RuleFor(x => x.ISBN)
				.NotEmpty().WithMessage(ValidationMessages.BookISBNRequired)
				.Matches(@"^\d{10}(\d{3})?$").WithMessage(ValidationMessages.BookISBNInvalidFormat);

			RuleFor(x => x.Pages)
				.GreaterThan(0).WithMessage(ValidationMessages.BookPagesGreaterThanZero);

			RuleFor(x => x.PublicationYear)
				.GreaterThan(1000).WithMessage(ValidationMessages.BookPublicationYearInvalid)
				.LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage(ValidationMessages.BookPublicationYearInvalidMaxYear);

			RuleFor(x => x.TotalCopies)
				.GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.BookTotalCopiesNegative);

			RuleFor(x => x.Description)
				.MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Description))
				.WithMessage(ValidationMessages.BookDescriptionMaxLength);

			RuleFor(x => x.GenreIds)
				.NotEmpty().WithMessage(ValidationMessages.BookGenreRequired);
		}
	}
}
