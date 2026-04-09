namespace Library.Application.Users.Queries.GetAllUsers
{
	public record GetAllUsersQuery : IRequest<List<UserListDto>>;
}
