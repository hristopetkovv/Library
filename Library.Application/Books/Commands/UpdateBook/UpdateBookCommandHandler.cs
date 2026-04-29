namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.Id, cancellationToken, b => b.Genres);
			if (book is null)
				throw new NotFoundException(ValidationMessages.BookNotFound);

			var authorExist = await unitOfWork.Authors.AnyAsync(a => a.Id == command.AuthorId, cancellationToken);
			if (!authorExist)
				throw new NotFoundException(ValidationMessages.AuthorNotFound);

			var publisherExist = await unitOfWork.Publishers.AnyAsync(p => p.Id == command.PublisherId, cancellationToken);
			if (!publisherExist)
				throw new NotFoundException(ValidationMessages.PublisherNotFound);

			book.Update(
				command.Title, 
				command.AuthorId, 
				command.PublisherId, 
				ISBN.Create(command.ISBN), 
				command.Description, 
				command.Pages, 
				command.Language, 
				command.CoverType, 
				command.PublicationYear, 
				command.TotalCopies,
				command.AvailableCopies
			);

			var newGenres = await unitOfWork.Genres.GetAllFilteredAsync(e => command.GenreIds.Contains(e.Id), cancellationToken);
			book.Genres.Clear();
			foreach (var genre in newGenres)
			{
				book.Genres.Add(genre);
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return book.Adapt<BookDetailDto>();
		}
	}
}
