namespace Library.Controllers.Books
{
	[ApiController]
	[Route("api/[controller]")]
	public class BooksController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> GetAll(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllBooksQuery(), cancellationToken));

		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<BookDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBookByIdQuery(id), cancellationToken));

		[HttpGet("available")]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> GetAvailable(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAvailableBooksQuery(), cancellationToken));

		[HttpGet("search")]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> Search([FromQuery] string term, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new SearchBooksQuery(term), cancellationToken));

		[HttpPost]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<BookDetailDto>> CreateBook([FromBody] CreateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await mediator.Send(command, cancellationToken);

			return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
		}

		[HttpPut("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<BookDetailDto>> Update([FromRoute] int id, [FromBody] UpdateBookRequest request, CancellationToken cancellationToken)
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
