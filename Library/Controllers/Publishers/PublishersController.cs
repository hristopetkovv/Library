namespace Library.Controllers.Publishers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PublishersController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<PublisherListDto>>> GetAll([FromQuery] string publisherName, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllPublishersQuery(publisherName), cancellationToken));

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<PublisherDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetPublisherByIdQuery(id), cancellationToken));

		[HttpPost]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Create([FromBody] CreatePublisherRequest request, CancellationToken cancellationToken)
		{
            var command = request.Adapt<CreatePublisherCommand>();

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpPut("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePublisherRequest request, CancellationToken cancellationToken)
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
