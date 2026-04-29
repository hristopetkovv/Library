namespace Library.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<UpdateUserCommand, UserDetailDto>
	{
		public async Task<UserDetailDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdForUpdateAsync(userContext.UserId, cancellationToken);
			if (user is null)
				throw new NotFoundException(ValidationMessages.UserNotFound);

			var emailExists = await unitOfWork.Users.AnyAsync(u => u.Email.Value == command.Email && u.Id != userContext.UserId, cancellationToken);
			if (emailExists)
				throw new BadRequestException(ValidationMessages.UserEmailExists);

			user.Update(
				Email.Create(command.Email), 
				FullName.Create(command.FirstName, command.LastName), 
				!string.IsNullOrWhiteSpace(command.Address) && !string.IsNullOrWhiteSpace(command.PhoneNumber)
					? ContactInfo.Create(command.Address, command.PhoneNumber)
					: null
				);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return user.Adapt<UserDetailDto>();
		}
	}
}
