namespace Library.Application.Borrowings.Queries.GetBorrowingHistoryByUserId
{
	public record GetBorrowingHistoryByUserIdQuery(int UserId) : IRequest<List<BorrowingDto>>;
}
