namespace Library.Application.Users.Commands.UpdateUserRole
{
	public class UpdateUserRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRoleCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (user is null)
				throw new NotFoundException($"User with ID {command.Id} not found.");

			if (user.Role != UserRole.Admin)
				throw new ForbiddenException("Only admins can change user roles");

			user.UpdateRole(command.NewRole);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
