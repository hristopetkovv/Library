namespace Library.Application.Borrowings.Queries.GetActiveBorrowingsByUserId
{
	public class GetActiveBorrowingsByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetActiveBorrowingsByUserIdQuery, List<BorrowingDto>>
	{
		public async Task<List<BorrowingDto>> Handle(GetActiveBorrowingsByUserIdQuery query, CancellationToken cancellationToken)
		{
			var activeBorrowings = await unitOfWork.Borrowings.GetActiveBorrowingsByUserIdAsync(query.UserId, cancellationToken);

			return [.. activeBorrowings.Select(b => new BorrowingDto(
				b.Id,
				b.Book.Adapt<BookListDto>(),
				b.User.Email.Value,
				b.BorrowDate,
				b.DueDate,
				b.ReturnDate,
				b.Status
			))];
		}
	}
}
