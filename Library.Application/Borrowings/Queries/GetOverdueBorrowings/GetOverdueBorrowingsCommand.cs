namespace Library.Application.Borrowings.Queries.GetOverdueBorrowings
{
	public record GetOverdueBorrowingsCommand() : IRequest<List<BorrowingDto>>;
}
