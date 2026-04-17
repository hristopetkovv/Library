namespace Library.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<UpdateUserCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
		{
			var userId = userContext.GetUserId();

			if (command.Id != userId)
				throw new ForbiddenException("You can only update your own profile.");

			var user = await unitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
			if (user is null)
				throw new NotFoundException($"User with ID {userId} not found.");

			var emailExists = await unitOfWork.Users.AnyAsync(u => u.Email.Value == command.Email && u.Id != userId, cancellationToken);
			if (emailExists)
				throw new BadRequestException($"Email '{command.Email}' is already in use by another user.");

			user.Update(
				Email.Create(command.Email), 
				FullName.Create(command.FirstName, command.LastName), 
				!string.IsNullOrWhiteSpace(command.Address) && !string.IsNullOrWhiteSpace(command.PhoneNumber)
					? ContactInfo.Create(command.Address, command.PhoneNumber)
					: null
				);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
