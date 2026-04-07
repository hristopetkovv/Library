namespace Library.Application.Books.Queries.GetAvailableBooks
{
	public record GetAvailableBooksQuery : IRequest<List<BookListDto>>;
}
