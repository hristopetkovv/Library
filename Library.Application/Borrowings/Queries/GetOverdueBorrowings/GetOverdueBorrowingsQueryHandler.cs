namespace Library.Application.Borrowings.Queries.GetOverdueBorrowings
{
	public class GetOverdueBorrowingsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOverdueBorrowingsQuery, List<BorrowingDetailDto>>
	{
		public async Task<List<BorrowingDetailDto>> Handle(GetOverdueBorrowingsQuery query, CancellationToken cancellationToken)
		{
			var overdueBorrowings = await unitOfWork.Borrowings.GetOverdueBorrowingsAsync(cancellationToken);

			return [.. overdueBorrowings.Select(b => new BorrowingDetailDto(
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
