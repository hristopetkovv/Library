namespace Library.Application.Borrowings.Queries.GetBorrowingHistoryByUserId
{
	public class GetBorrowingHistoryByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBorrowingHistoryByUserIdQuery, List<BorrowingDetailDto>>
	{
		public async Task<List<BorrowingDetailDto>> Handle(GetBorrowingHistoryByUserIdQuery query, CancellationToken cancellationToken)
		{
			var historyBorrowings = await unitOfWork.Borrowings.GetBorrowingHistoryByUserAsync(query.UserId, cancellationToken);

			return [.. historyBorrowings.Select(b => new BorrowingDetailDto(
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
