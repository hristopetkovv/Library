namespace Library.Application.Borrowings.Commands.ReturnBook
{
	public class ReturnBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReturnBookCommand, Unit>
	{
		public async Task<Unit> Handle(ReturnBookCommand command, CancellationToken cancellationToken)
		{
			var borrowing = await unitOfWork.Borrowings.GetByIdForUpdateAsync(command.BorrowingId, cancellationToken, b => b.Book);
			if (borrowing == null)
				throw new NotFoundException(nameof(Borrowing), command.BorrowingId);

			await unitOfWork.BeginTransactionAsync(cancellationToken);

			try
			{
				borrowing.MarkAsReturned();

				borrowing.Book.IncrementAvailableCopies();

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
