namespace Library.Application.Borrowings.Queries.GetOverdueBorrowings
{
	public class GetOverdueBorrowingsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOverdueBorrowingsCommand, List<BorrowingDto>>
	{
		public async Task<List<BorrowingDto>> Handle(GetOverdueBorrowingsCommand query, CancellationToken cancellationToken)
		{
			var overdueBorrowings = await unitOfWork.Borrowings.GetOverdueBorrowingsAsync(cancellationToken);

			return [.. overdueBorrowings.Select(b => new BorrowingDto(
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
