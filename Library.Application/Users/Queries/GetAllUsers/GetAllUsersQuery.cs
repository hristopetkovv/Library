namespace Library.Application.Users.Queries.GetAllUsers
{
	public record GetAllUsersQuery(SearchUsersFilterDto? Filter = null) : IRequest<List<UserListDto>>;
}
