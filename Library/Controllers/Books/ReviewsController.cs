namespace Library.Controllers.Books
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("book/{bookId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReviewDto>>> GetByBookId([FromRoute] int bookId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBookReviewsQuery(bookId), cancellationToken));

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin, UserRole.Member)]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new CreateReviewCommand(request.BookId, request.Content, request.Rating), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{bookId:int}/{reviewId:int}")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> Delete([FromRoute] int bookId, [FromRoute] int reviewId, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteReviewCommand(bookId, reviewId), cancellationToken);
            return NoContent();
        }
    }
}
