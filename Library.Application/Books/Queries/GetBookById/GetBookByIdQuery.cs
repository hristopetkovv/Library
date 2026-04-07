namespace Library.Application.Books.Queries.GetBookById
{
	public record GetBookByIdQuery(int Id) : IRequest<BookDetailDto>;
}
