namespace Library.Application.Borrowings.Commands.BorrowBook
{
	public record BorrowBookCommand(
		int BookId,
		int UserId
	) : IRequest<Unit>;
}
