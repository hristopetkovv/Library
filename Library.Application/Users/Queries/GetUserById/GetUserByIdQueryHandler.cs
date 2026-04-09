namespace Library.Application.Users.Queries.GetUserById
{
	public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, UserDto>
	{
		public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
		{
			var user = await unitOfWork.Users.GetByIdAsync(query.Id, cancellationToken);
			if (user == null)
				throw new NotFoundException($"User with ID {query.Id} not found.");

			return user.Adapt<UserDto>();
		}
	}
}
