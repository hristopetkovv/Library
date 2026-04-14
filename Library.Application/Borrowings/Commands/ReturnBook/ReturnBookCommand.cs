namespace Library.Application.Borrowings.Commands.ReturnBook
{
	public record ReturnBookCommand(int BorrowingId) : ICommand<Unit>;
}
