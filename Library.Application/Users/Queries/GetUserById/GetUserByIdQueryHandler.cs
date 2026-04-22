namespace Library.Application.Users.Queries.GetUserById
{
	public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<GetUserByIdQuery, UserDetailDto>
	{
		public async Task<UserDetailDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(query.Id, cancellationToken);
			if (user is null)
				throw new NotFoundException($"User with ID {query.Id} not found.");

			if (query.Id != userContext.UserId && user?.Role != UserRole.Admin)
				throw new ForbiddenException("You can only view your own profile");

			return user.Adapt<UserDetailDto>();
		}
	}
}
