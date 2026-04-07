namespace Library.Application.Borrowings.Queries.GetActiveBorrowingsByUserId
{
	public class GetActiveBorrowingsByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetActiveBorrowingsByUserIdQuery, List<BorrowingDto>>
	{
		public async Task<List<BorrowingDto>> Handle(GetActiveBorrowingsByUserIdQuery query, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(query.UserId, cancellationToken);
			if (user == null)
				throw new NotFoundException(nameof(User), query.UserId);

			var activeBorrowings = await unitOfWork.Borrowings.GetActiveBorrowingsByUserIdAsync(query.UserId, cancellationToken);

			return [.. activeBorrowings.Select(b => new BorrowingDto(
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
