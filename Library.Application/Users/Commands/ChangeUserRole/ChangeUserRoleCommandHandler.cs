namespace Library.Application.Users.Commands.ChangeUserRole
{
	public class ChangeUserRoleCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<ChangeUserRoleCommand, Unit>
	{
		public async Task<Unit> Handle(ChangeUserRoleCommand command, CancellationToken cancellationToken)
		{
			if (userContext.UserId == command.Id)
				throw new ForbiddenException("Users cannot change your own role.");

			var user = await unitOfWork.Users.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (user is null)
				throw new NotFoundException($"User with ID {command.Id} not found.");

			user.UpdateRole(command.NewRole);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
