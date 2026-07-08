namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IUnitOfWork unitOfWork, ICoverService coverService) : IRequestHandler<UpdateBookCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await ValidatePropertiesExisting(command, cancellationToken);

            var coverImage = await coverService.TryDownloadCoverAsync(command.ISBN);

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
				command.AvailableCopies,
                coverImage,
				command.GenreIds
            );

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private async Task<Book> ValidatePropertiesExisting(UpdateBookCommand command, CancellationToken cancellationToken)
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

			return book;
		}
	}
}
