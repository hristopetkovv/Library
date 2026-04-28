namespace Library.Application.Users.Commands.ForgottenPassword
{
	public class ForgottenPasswordCommandHandler : IRequestHandler<ForgottenPasswordCommand, Unit>
	{
		public async Task<Unit> Handle(ForgottenPasswordCommand request, CancellationToken cancellationToken)
		{
			// TODO: get user and send email 

			return Unit.Value;
		}
	}
}
