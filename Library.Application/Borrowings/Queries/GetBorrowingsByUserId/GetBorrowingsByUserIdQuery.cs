namespace Library.Application.Borrowings.Queries.GetBorrowingsByUserId
{
	public record GetBorrowingsByUserIdQuery(int UserId, BorrowingStatus? Status) : IRequest<List<BorrowingBasicDto>>;
}
