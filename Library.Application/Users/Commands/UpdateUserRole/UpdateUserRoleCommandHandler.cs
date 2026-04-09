namespace Library.Application.Users.Commands.UpdateUserRole
{
	public class UpdateUserRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRoleCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (user == null)
				throw new NotFoundException($"User with ID {command.Id} not found.");

			user.UpdateRole(command.NewRole);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
