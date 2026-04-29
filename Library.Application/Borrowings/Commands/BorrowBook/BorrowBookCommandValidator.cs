namespace Library.Application.Borrowings.Commands.BorrowBook
{
	public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
	{
		public BorrowBookCommandValidator()
		{
			RuleFor(x => x.BookId)
				.GreaterThan(0).WithMessage(ValidationMessages.BookInvalidId);

			RuleFor(x => x.UserId)
				.GreaterThan(0).WithMessage(ValidationMessages.UserInvalidId);
		}
	}
}
