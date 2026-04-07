namespace Library.Application.Borrowings.Commands.ReturnBook
{
	public record ReturnBookCommand(int BorrowingId) : IRequest<Unit>;
}
