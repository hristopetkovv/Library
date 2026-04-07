namespace Library.Application.Borrowings.Queries.GetBorrowingHistoryByUserId
{
	public class GetBorrowingHistoryByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBorrowingHistoryByUserIdQuery, List<BorrowingDto>>
	{
		public async Task<List<BorrowingDto>> Handle(GetBorrowingHistoryByUserIdQuery query, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(query.UserId, cancellationToken);
			if (user == null)
				throw new NotFoundException(nameof(User), query.UserId);

			var historyBorrowings = await unitOfWork.Borrowings.GetBorrowingHistoryByUserAsync(query.UserId, cancellationToken);

			return [.. historyBorrowings.Select(b => new BorrowingDto(
				b.Id,
				b.Book.Adapt<BookListDto>(),
				user.Email.Value,
				b.BorrowDate,
				b.DueDate,
				b.ReturnDate,
				b.Status
			))];
		}
	}
}
