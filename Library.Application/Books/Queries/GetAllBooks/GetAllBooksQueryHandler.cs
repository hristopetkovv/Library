namespace Library.Application.Books.Queries.GetAllBooks
{
	public class GetAllBooksQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllBooksQuery, List<BookListDto>>
	{
		public async Task<List<BookListDto>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
		{
			var books = await unitOfWork.Books.GetAllAsync(cancellationToken, b => b.Author, b => b.Publisher);

			return books.Adapt<List<BookListDto>>();
		}
	}
}
