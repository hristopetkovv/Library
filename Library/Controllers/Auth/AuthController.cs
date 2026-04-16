namespace Library.Controllers.Auth
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
			=> Ok(await authService.LoginAsync(request, cancellationToken));

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
		{
			await authService.RegisterAsync(request, cancellationToken);

			return Ok();
		}
	}
}
