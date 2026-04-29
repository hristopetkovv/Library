namespace Library.Application.Borrowings.Commands.BorrowBook
{
	public class BorrowBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BorrowBookCommand, Unit>
	{
		public async Task<Unit> Handle(BorrowBookCommand command, CancellationToken cancellationToken)
		{
			var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.BookId, cancellationToken);
			var user = await unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken, u => u.Borrowings);

			ValidateBorrowing(command, user, book);

			await unitOfWork.BeginTransactionAsync(cancellationToken);

			try
			{
				var borrowing = Borrowing.Create(command.BookId, command.UserId);

				book!.DecrementAvailableCopies();

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

		private void ValidateBorrowing(BorrowBookCommand command, User? user, Book? book)
		{
			if (book is null)
				throw new NotFoundException(nameof(Book), command.BookId);

			if (!book.CanBeBorrowed())
				throw new BadRequestException(ValidationMessages.BookHasNoAvailableCopies);

			if (user is null)
				throw new NotFoundException(ValidationMessages.UserNotFound);

			if (!user.CanBorrow())
				throw new BadRequestException(ValidationMessages.UserCannotBorrowMore);

			if (user.HasOverdueBooks())
				throw new BadRequestException(ValidationMessages.UserHasOverdueBooks);
		}
	}
}
