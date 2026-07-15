namespace Library.Application.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeactivateUserCommand, Unit>
    {
        public async Task<Unit> Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException(ValidationMessages.UserNotFound);

            user.Deactivate();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
