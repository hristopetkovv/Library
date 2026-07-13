namespace Library.Application.Users.Queries.GetAllUsers
{
	public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, List<UserListDto>>
	{
		public async Task<List<UserListDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
		{
			var users = await unitOfWork.Users.GetAllFilteredAsync(query.Filter!.Predicate(), cancellationToken);

			return users.Adapt<List<UserListDto>>();
		}
	}
}
