namespace Library.Application.Users.Commands.DeactivateUser
{
    public record DeactivateUserCommand(int UserId) : IRequest<Unit>;
}
