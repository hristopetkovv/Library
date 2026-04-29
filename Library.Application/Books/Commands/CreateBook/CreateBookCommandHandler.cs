namespace Library.Application.Books.Commands.CreateBook
{
	public class CreateBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(command.AuthorId, cancellationToken);
			if (author is null)
				throw new NotFoundException(ValidationMessages.AuthorNotFound);

			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.PublisherId, cancellationToken);
			if (publisher is null)
				throw new NotFoundException(ValidationMessages.PublisherNotFound);

			var book = Book.Create(
				command.Title, 
				command.AuthorId, 
				command.PublisherId, 
				ISBN.Create(command.ISBN), 
				command.Description, 
				command.Pages, 
				command.Language, 
				command.CoverType, 
				command.PublicationYear, 
				command.TotalCopies
			);

			var genres = await unitOfWork.Genres.GetAllFilteredAsync(e => command.GenreIds.Contains(e.Id), cancellationToken);
			foreach ( var genre in genres )
			{
				book.AddGenre(genre);
			}

			await unitOfWork.Books.AddAsync(book, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return book.Adapt<BookDetailDto>();
		}
	}
}
