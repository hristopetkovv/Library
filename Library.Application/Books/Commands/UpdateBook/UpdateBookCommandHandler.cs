namespace Library.Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateBookCommand, BookDetailDto>
	{
		public async Task<BookDetailDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (book is null)
				throw new NotFoundException(nameof(Book), command.Id);

			var authorExist = await unitOfWork.Authors.AnyAsync(a => a.Id == command.AuthorId, cancellationToken);
			if (!authorExist)
				throw new NotFoundException(nameof(Author), command.AuthorId);

			var publisherExist = await unitOfWork.Publishers.AnyAsync(p => p.Id == command.PublisherId, cancellationToken);
			if (!publisherExist)
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
				command.TotalCopies,
				command.AvailableCopies
			);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return book.Adapt<BookDetailDto>();
		}
	}
}
