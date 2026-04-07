namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
	{
		public UpdateBookCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid Book ID is required");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required")
				.MaximumLength(300).WithMessage("Title cannot exceed 300 characters");

			RuleFor(x => x.AuthorId)
				.GreaterThan(0).WithMessage("Valid Author ID is required");

			RuleFor(x => x.PublisherId)
				.GreaterThan(0).WithMessage("Valid Publisher ID is required");

			RuleFor(x => x.ISBN)
				.NotEmpty().WithMessage("ISBN is required")
				.Matches(@"^\d{10}(\d{3})?$").WithMessage("Invalid ISBN format");

			RuleFor(x => x.Pages)
				.GreaterThan(0).WithMessage("Pages must be greater than zero");

			RuleFor(x => x.PublicationYear)
				.GreaterThan(1000).WithMessage("Publication year must be valid")
				.LessThanOrEqualTo(DateTime.UtcNow.Year);

			RuleFor(x => x.TotalCopies)
				.GreaterThanOrEqualTo(0).WithMessage("Total copies cannot be negative");

			RuleFor(x => x.Description)
				.MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Description));
		}
	}
}
