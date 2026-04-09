namespace Library.Application.Books.Commands.DeleteBook
{
	public class DeleteBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(command.Id, cancellationToken, b => b.Borrowings);
			if (book == null)
				throw new NotFoundException(nameof(Book), command.Id);

			if (book.Borrowings.Any(b => b.Status == BorrowingStatus.Borrowed))
				throw new InvalidOperationException("Cannot delete a book with active borrowings.");

			unitOfWork.Books.Remove(book);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
