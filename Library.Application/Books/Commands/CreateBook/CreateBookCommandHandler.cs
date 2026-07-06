namespace Library.Application.Books.Commands.CreateBook
{
	public class CreateBookCommandHandler(IUnitOfWork unitOfWork, ICoverService coverService) : IRequestHandler<CreateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
		{
			await ValidatePropertiesExisting(command, cancellationToken);

			var coverImage = await coverService.TryDownloadCoverAsync(command.ISBN);

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
				command.TotalCopies,
                coverImage,
				command.GenreIds
            );

			await unitOfWork.Books.AddAsync(book, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return book.Adapt<BookDetailDto>();
		}

		private async Task ValidatePropertiesExisting(CreateBookCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(command.AuthorId, cancellationToken);
			if (author is null)
				throw new NotFoundException(ValidationMessages.AuthorNotFound);

			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.PublisherId, cancellationToken);
			if (publisher is null)
				throw new NotFoundException(ValidationMessages.PublisherNotFound);
		}
	}
}
