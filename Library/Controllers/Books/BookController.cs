namespace Library.Controllers.Books
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> GetAllBooks(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllBooksQuery(), cancellationToken));

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<ActionResult<BookDetailDto>> GetBookById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBookByIdQuery(id), cancellationToken));

		[HttpGet("available")]
		[AllowAnonymous]
		public async Task<ActionResult<List<BookListDto>>> GetAvailableBooks(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAvailableBooksQuery(), cancellationToken));

		[HttpGet("search")]
		public async Task<ActionResult<List<BookListDto>>> SearchBooks([FromQuery] string term, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new SearchBooksQuery(term), cancellationToken));

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<BookDetailDto>> CreateBook([FromBody] CreateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await mediator.Send(command, cancellationToken);

			return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<BookDetailDto>> UpdateBook(UpdateBookCommand command, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(command, cancellationToken));

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteBook([FromRoute] int id, CancellationToken cancellationToken)
		{
			await mediator.Send(new DeleteBookCommand(id), cancellationToken);

			return NoContent();
		}
	}
}
