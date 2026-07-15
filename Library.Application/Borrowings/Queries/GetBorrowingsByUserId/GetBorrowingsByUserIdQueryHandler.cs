namespace Library.Application.Borrowings.Queries.GetBorrowingsByUserId
{
	public class GetBorrowingsByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBorrowingsByUserIdQuery, List<BorrowingBasicDto>>
	{
		public async Task<List<BorrowingBasicDto>> Handle(GetBorrowingsByUserIdQuery query, CancellationToken cancellationToken)
		{
			var borrowings = await unitOfWork.Borrowings.GetByUserIdAsync(query.UserId, query.Status, cancellationToken);

			return borrowings.Adapt<List<BorrowingBasicDto>>();
		}
	}
}
