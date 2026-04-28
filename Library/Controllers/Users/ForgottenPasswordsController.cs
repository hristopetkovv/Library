namespace Library.Controllers.Users
{
	[ApiController]
	[Route("api/[controller]")]
	public class ForgottenPasswordsController(IMediator mediator) : ControllerBase
	{
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword([FromBody] string email)
			=> Ok(await mediator.Send(new ForgottenPasswordCommand(email)));
	}
}
