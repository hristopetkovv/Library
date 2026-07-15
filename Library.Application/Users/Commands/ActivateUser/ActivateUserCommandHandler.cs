namespace Library.Application.Users.Commands.ActivateUser
{
    public class ActivateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ActivateUserCommand, Unit>
    {
        public async Task<Unit> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException(ValidationMessages.UserNotFound);

            user.Activate();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
