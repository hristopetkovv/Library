namespace Library.Application.Users.Commands.ActivateUser
{
    public record ActivateUserCommand(int UserId) : ICommand<Unit>;
}
