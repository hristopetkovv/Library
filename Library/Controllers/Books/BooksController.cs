namespace Library.Controllers.Books
{
	[ApiController]
	[Route("api/[controller]")]
	public class BooksController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> GetAll([FromQuery] SearchBooksFilterDto filter, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllBooksQuery(filter), cancellationToken));

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<BookDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBookByIdQuery(id), cancellationToken));

		[HttpPost]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<CreateBookCommand>();

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpPut("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBookRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<UpdateBookCommand>() with { Id = id };

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpDelete("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{
			await mediator.Send(new DeleteBookCommand(id), cancellationToken);

			return NoContent();
		}
	}
}
