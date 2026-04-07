namespace Library.Application.Books.Queries.GetAvailableBooks
{
	public class GetAvailableBooksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvailableBooksQuery, List<BookListDto>>
	{
		public async Task<List<BookListDto>> Handle(GetAvailableBooksQuery query, CancellationToken cancellationToken)
		{
			var books = await unitOfWork.Books.GetAvailableBooksAsync(cancellationToken);

			return books.Adapt<List<BookListDto>>();
		}
	}
}
