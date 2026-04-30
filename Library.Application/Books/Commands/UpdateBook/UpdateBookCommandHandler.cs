namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : IRequestHandler<UpdateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await ValidatePropertiesExisting(command, cancellationToken);

			var coverImageUrl = await GetNewCoverImageUrl(command.CoverImage, book, cancellationToken);

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
				coverImageUrl
			);

			await AddGenres(book, command.GenreIds, cancellationToken);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return book.Adapt<BookDetailDto>();
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

		private async Task<string?> GetNewCoverImageUrl(FileUploadDto? newCoverImage, Book book, CancellationToken cancellationToken)
		{
			if (newCoverImage is not null)
			{
				if (!string.IsNullOrEmpty(book.CoverImageUrl))
					await fileStorageService.DeleteFileAsync(book.CoverImageUrl, cancellationToken);

				var newCoverImageUrl = await fileStorageService.SaveFileAsync(newCoverImage.Content, newCoverImage.FileName, newCoverImage.ContentType, cancellationToken);

				return newCoverImageUrl;
			}

			return book.CoverImageUrl;
		}

		private async Task AddGenres(Book book, List<int> genreIds, CancellationToken cancellationToken)
		{
			var newGenres = await unitOfWork.Genres.GetAllFilteredAsync(e => genreIds.Contains(e.Id), cancellationToken);
			book.Genres.Clear();
			foreach (var genre in newGenres)
			{
				book.Genres.Add(genre);
			}
		}
	}
}
