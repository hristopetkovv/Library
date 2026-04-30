namespace Library.Application.Books.Commands.CreateBook
{
	public class CreateBookCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : IRequestHandler<CreateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
		{
			await ValidatePropertiesExisting(command, cancellationToken);

			var coverImageUrl = await GetCoverImageUrl(command.CoverImage, cancellationToken);

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
				coverImageUrl
			);

			await AddGenres(book, command.GenreIds, cancellationToken);

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

		private async Task<string?> GetCoverImageUrl(FileUploadDto? coverImage, CancellationToken cancellationToken)
		{
			string? coverImageUrl = null;

			if (coverImage is not null)
				coverImageUrl = await fileStorageService.SaveFileAsync(
					coverImage.Content,
					coverImage.FileName,
					coverImage.ContentType,
					cancellationToken);

			return coverImageUrl;
		}

		private async Task AddGenres(Book book, List<int> genreIds, CancellationToken cancellationToken)
		{
			var genres = await unitOfWork.Genres.GetAllFilteredAsync(e => genreIds.Contains(e.Id), cancellationToken);
			foreach (var genre in genres)
			{
				book.AddGenre(genre);
			}
		}
	}
}
