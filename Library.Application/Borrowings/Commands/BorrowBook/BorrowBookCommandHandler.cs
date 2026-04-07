namespace Library.Application.Borrowings.Commands.BorrowBook
{
	public class BorrowBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BorrowBookCommand, Unit>
	{
		public async Task<Unit> Handle(BorrowBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdAsync(command.BookId, cancellationToken);
			if (book == null)
				throw new NotFoundException(nameof(Book), command.BookId);

			if (!book.CanBeBorrowed())
				throw new InvalidOperationException("Book is not available for borrowing.");

			var user = await unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken);
			if (user == null)
				throw new NotFoundException(nameof(User), command.UserId);

			if (!user.CanBorrow())
				throw new InvalidOperationException("User cannot borrow more books.");

			await unitOfWork.BeginTransactionAsync(cancellationToken);

			try
			{
				var borrowing = Borrowing.Create(command.BookId, command.UserId);

				book.DecrementAvailableCopies();

				await unitOfWork.Borrowings.AddAsync(borrowing, cancellationToken);
				await unitOfWork.SaveChangesAsync(cancellationToken);
				await unitOfWork.CommitTransactionAsync(cancellationToken);

				return Unit.Value;
			}
			catch
			{
				await unitOfWork.RollbackTransactionAsync(cancellationToken);
				throw;
			}
		}
	}
}
