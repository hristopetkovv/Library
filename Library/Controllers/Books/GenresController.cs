namespace Library.Controllers.Books
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDto>>> GetAll(CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetAllGenresQuery(), cancellationToken));
    }
}
