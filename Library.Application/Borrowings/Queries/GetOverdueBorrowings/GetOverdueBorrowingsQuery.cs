namespace Library.Application.Borrowings.Queries.GetOverdueBorrowings
{
	public record GetOverdueBorrowingsQuery() : IRequest<List<BorrowingDetailDto>>;
}
