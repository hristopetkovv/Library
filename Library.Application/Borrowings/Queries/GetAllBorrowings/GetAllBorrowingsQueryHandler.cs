namespace Library.Application.Borrowings.Queries.GetAllBorrowings
{
	public class GetAllBorrowingsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllBorrowingsQuery, List<BorrowingDetailDto>>
	{
		public async Task<List<BorrowingDetailDto>> Handle(GetAllBorrowingsQuery query, CancellationToken cancellationToken)
		{
			var borrowings = await unitOfWork.Borrowings.GetBorrowingsAsync(query.Filter!.Predicate(), cancellationToken);

            return borrowings.Adapt<List<BorrowingDetailDto>>();
        }
	}
}
