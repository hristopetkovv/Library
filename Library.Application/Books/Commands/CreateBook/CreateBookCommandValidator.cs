namespace Library.Application.Books.Commands.CreateBook
{
	public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
	{
		public CreateBookCommandValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required")
				.MaximumLength(300).WithMessage("Title cannot exceed 300 characters");

			RuleFor(x => x.AuthorId)
				.GreaterThan(0).WithMessage("Valid Author ID is required");

			RuleFor(x => x.PublisherId)
				.GreaterThan(0).WithMessage("Valid Publisher ID is required");

			RuleFor(x => x.ISBN)
				.NotEmpty().WithMessage("ISBN is required")
				.Matches(@"^\d{10}(\d{3})?$").WithMessage("Invalid ISBN format (must be 10 or 13 digits)");

			RuleFor(x => x.Pages)
				.GreaterThan(0).WithMessage("Pages must be greater than zero");

			RuleFor(x => x.PublicationYear)
				.GreaterThan(1000).WithMessage("Publication year must be valid")
				.LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Publication year cannot be in the future");

			RuleFor(x => x.TotalCopies)
				.GreaterThanOrEqualTo(0).WithMessage("Total copies cannot be negative");

			RuleFor(x => x.Description)
				.MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Description))
				.WithMessage("Description cannot exceed 2000 characters");
		}
	}
}
