namespace Library.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(command.Id, cancellationToken);
			if (user == null)
				throw new NotFoundException($"User with ID {command.Id} not found.");

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
