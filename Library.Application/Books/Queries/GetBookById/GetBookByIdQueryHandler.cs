namespace Library.Application.Books.Queries.GetBookById
{
	public class GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<GetBookByIdQuery, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(query.Id, cancellationToken, b => b.Author, b => b.Publisher);
			if (book is null)
				throw new NotFoundException(nameof(Book), query.Id);

            var isFavorite = await unitOfWork.Users.IsFavoriteAsync(userContext.UserId, query.Id, cancellationToken);

            var bookDto = book.Adapt<BookDetailDto>() with { IsFavorite = isFavorite };

            return bookDto;
        }
	}
}
