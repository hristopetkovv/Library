namespace Library.Controllers.Publishers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PublishersController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<PublisherListDto>>> GetAll(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllPublishersQuery(), cancellationToken));

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<PublisherDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetPublisherByIdQuery(id), cancellationToken));

		[HttpPost]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<PublisherDetailDto>> Create([FromBody] CreatePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await mediator.Send(command, cancellationToken);

			return CreatedAtAction(nameof(GetById), new { id = publisher.Id }, publisher);
		}

		[HttpPut("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<PublisherDetailDto>> Update([FromRoute] int id, [FromBody] UpdatePublisherRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<UpdatePublisherCommand>() with { Id = id };

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpDelete("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{
			await mediator.Send(new DeletePublisherCommand(id), cancellationToken);

			return NoContent();
		}
	}
}
