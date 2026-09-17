namespace Library.Controllers.Books
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteBooksController(IMediator mediator) : ControllerBase
    {
        [HttpGet("mine")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Member)]
        public async Task<ActionResult<List<UserFavoriteBookDto>>> GetMine(CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetFavoriteBooksQuery(), cancellationToken));

        [HttpPost("{bookId:int}")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Member)]
        public async Task<IActionResult> Add([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            await mediator.Send(new AddBookToFavoritesCommand(bookId), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{bookId:int}")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Member)]
        public async Task<IActionResult> Remove([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoveBookFromFavoritesCommand(bookId), cancellationToken);
            return NoContent();
        }
    }
}
