namespace Library.Application.Books.Queries.GetAllBooks
{
	public class GetAllBooksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllBooksQuery, List<BookListDto>>
	{
		public async Task<List<BookListDto>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
		{
			var books = await unitOfWork.Books.GetAllFilteredAsync(query.Filter!.Predicate(), cancellationToken, b => b.Author, b => b.Publisher, b => b.Genres);

			return books.Adapt<List<BookListDto>>();
		}
	}
}
