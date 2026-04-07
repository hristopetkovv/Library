namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(command.Id, cancellationToken);
			if (book == null)
				throw new NotFoundException(nameof(Book), command.Id);

			var author = await unitOfWork.Authors.GetByIdAsync(command.AuthorId, cancellationToken);
			if (author == null)
				throw new NotFoundException(nameof(Author), command.AuthorId);

			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.PublisherId, cancellationToken);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), command.PublisherId);

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
				command.TotalCopies
			);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			var updatedBook = await unitOfWork.Books
				.GetByIdAsync(command.Id, cancellationToken);

			return updatedBook.Adapt<BookDetailDto>()!;
		}
	}
}
