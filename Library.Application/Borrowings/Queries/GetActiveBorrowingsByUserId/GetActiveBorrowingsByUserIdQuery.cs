namespace Library.Application.Borrowings.Queries.GetActiveBorrowingsByUserId
{
	public record GetActiveBorrowingsByUserIdQuery(int UserId) : IRequest<List<BorrowingDto>>;
}
