namespace Library.Application.Books.Queries.SearchBooks
{
	public record SearchBooksQuery(string SearchTerm) : IRequest<List<BookListDto>>;
}
