namespace Library.Application.Users.Queries.GetUserById
{
	public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<GetUserByIdQuery, UserDetailDto>
	{
		public async Task<UserDetailDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(query.Id, cancellationToken);
			if (user is null)
				throw new NotFoundException(ValidationMessages.UserNotFound);

			if (query.Id != userContext.UserId && user?.Role != UserRole.Admin)
				throw new ForbiddenException(ValidationMessages.UserViewOwnProfileOnly);

			return user.Adapt<UserDetailDto>();
		}
	}
}
