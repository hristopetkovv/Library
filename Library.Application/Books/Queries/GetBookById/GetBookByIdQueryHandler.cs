namespace Library.Application.Books.Queries.GetBookById
{
	public class GetBookByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookByIdQuery, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(query.Id, cancellationToken);
			if (book == null)
				throw new NotFoundException(nameof(Book), query.Id);

			return book.Adapt<BookDetailDto>();
		}
	}
}
