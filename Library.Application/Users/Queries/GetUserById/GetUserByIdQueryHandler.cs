namespace Library.Application.Users.Queries.GetUserById
{
	public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<GetUserByIdQuery, UserDto>
	{
		public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
		{
			var userId = userContext.GetUserId();

			var user = await unitOfWork.Users.GetByIdAsync(query.Id, cancellationToken);
			if (user is null)
				throw new NotFoundException($"User with ID {query.Id} not found.");

			if (query.Id != userId && user?.Role != UserRole.Admin)
				throw new ForbiddenException("You can only view your own profile");

			return user.Adapt<UserDto>();
		}
	}
}
