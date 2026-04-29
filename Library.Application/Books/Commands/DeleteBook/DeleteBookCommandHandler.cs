namespace Library.Application.Books.Commands.DeleteBook
{
	public class DeleteBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(command.Id, cancellationToken, b => b.Borrowings);
			if (book is null)
				throw new NotFoundException(ValidationMessages.BookNotFound);

			if (book.Borrowings.Any(b => b.Status == BorrowingStatus.Borrowed))
				throw new BadRequestException(ValidationMessages.BookHasActiveBorrowings);

			unitOfWork.Books.Remove(book);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
