namespace Library.Application.Borrowings.Queries.GetAllActiveBorrowings
{
	public record GetAllActiveBorrowingsQuery() : IRequest<List<BorrowingDetailDto>>;
}
