namespace Library.Application.Borrowings.Queries.GetAllBorrowings
{
	public record GetAllBorrowingsQuery(SearchBorrowingsFilterDto Filter) : IRequest<List<BorrowingDetailDto>>;
}
