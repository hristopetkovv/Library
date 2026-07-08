namespace Library.Controllers.Authors
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthorsController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<AuthorListDto>>> GetAll([FromQuery] string authorName, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllAuthorsQuery(authorName), cancellationToken));

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<AuthorDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAuthorByIdQuery(id), cancellationToken));

		[HttpPost]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request, CancellationToken cancellationToken)
		{
            var command = request.Adapt<CreateAuthorCommand>();

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpPut("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAuthorRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<UpdateAuthorCommand>() with { Id = id };

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpDelete("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{
			await mediator.Send(new DeleteAuthorCommand(id), cancellationToken);

			return NoContent();
		}
	}
}
