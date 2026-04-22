namespace Library.Application.Borrowings.Queries.GetAllActiveBorrowings
{
	public class GetAllActiveBorrowingsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllActiveBorrowingsQuery, List<BorrowingDetailDto>>
	{
		public async Task<List<BorrowingDetailDto>> Handle(GetAllActiveBorrowingsQuery query, CancellationToken cancellationToken)
		{
			var activeBorrowings = await unitOfWork.Borrowings.GetAllActiveBorrowingsAsync(cancellationToken);

			return [.. activeBorrowings.Select(b => new BorrowingDetailDto(
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
