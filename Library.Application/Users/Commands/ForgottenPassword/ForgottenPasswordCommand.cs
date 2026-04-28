namespace Library.Application.Users.Commands.ForgottenPassword
{
	public record ForgottenPasswordCommand(string Email) : ICommand<Unit>;
}
