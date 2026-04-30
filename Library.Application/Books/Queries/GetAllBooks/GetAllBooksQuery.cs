namespace Library.Application.Books.Queries.GetAllBooks
{
	public record GetAllBooksQuery(SearchBooksFilterDto? Filter = null) : IRequest<List<BookListDto>>;
}
