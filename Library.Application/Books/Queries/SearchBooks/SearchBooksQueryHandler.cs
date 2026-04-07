namespace Library.Application.Books.Queries.SearchBooks
{
	public class SearchBooksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchBooksQuery, List<BookListDto>>
	{
		public async Task<List<BookListDto>> Handle(SearchBooksQuery query, CancellationToken cancellationToken)
		{
			var books = await unitOfWork.Books.SearchBooksAsync(query.SearchTerm, cancellationToken);

			return books.Adapt<List<BookListDto>>();
		}
	}
}
