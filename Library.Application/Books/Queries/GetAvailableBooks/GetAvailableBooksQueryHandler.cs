namespace Library.Application.Books.Queries.GetAvailableBooks
{
	public class GetAvailableBooksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAvailableBooksQuery, List<BookListDto>>
	{
		public async Task<List<BookListDto>> Handle(GetAvailableBooksQuery query, CancellationToken cancellationToken)
		{
			var books = await unitOfWork.Books.GetAllFilteredAsync(b => b.AvailableCopies > 0, cancellationToken, b => b.Author, b => b.Publisher);

			return books.Adapt<List<BookListDto>>();
		}
	}
}
