namespace Library.Controllers.Libraries
{
	[ApiController]
	[Route("api/[controller]")]
	public class LibrariesController(IMediator mediator) : ControllerBase
	{
		[HttpGet("stats")]
		[AllowAnonymous]
		public async Task<ActionResult<List<LibraryStatsDto>>> GetLibraryStats(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetLibraryStatsQuery(), cancellationToken));
	}
}
