namespace Library.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword) : ICommand<Unit>;
}
