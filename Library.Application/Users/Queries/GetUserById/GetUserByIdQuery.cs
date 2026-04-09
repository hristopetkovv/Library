namespace Library.Application.Users.Queries.GetUserById
{
	public record GetUserByIdQuery(int Id) : IRequest<UserDto>;
}
