namespace Library.Application.Users.Commands.DeleteUser
{
	public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(command.Id, cancellationToken, u => u.Borrowings);
			if (user is null)
				throw new NotFoundException(ValidationMessages.UserNotFound);

			if (user.Borrowings.Any(b => b.Status == BorrowingStatus.Borrowed))
				throw new BadRequestException(ValidationMessages.UserHasActiveBorrowings);

			unitOfWork.Users.Remove(user);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
