namespace Library.Application.Books.Queries.GetAllBooks
{
	public record GetAllBooksQuery : IRequest<List<BookListDto>>;
}
