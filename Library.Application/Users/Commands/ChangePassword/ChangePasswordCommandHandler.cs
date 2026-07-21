namespace Library.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IUserContext userContext) : IRequestHandler<ChangePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdForUpdateAsync(userContext.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException(ValidationMessages.UserNotFound);

            if (!passwordHasher.VerifyPassword(command.CurrentPassword, user.PasswordSalt, user.PasswordHash))
                throw new BadRequestException(ValidationMessages.InvalidCurrentPassword);

            var passwordHash = passwordHasher.HashPassword(command.NewPassword, out var passwordSalt);
            user.ChangePassword(passwordHash, passwordSalt);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
