namespace Library.Controllers.Borrowings
{
	[ApiController]
	[Route("api/[controller]")]
	public class BorrowingsController(IMediator mediator, IUserContext userContext) : ControllerBase
	{
		[HttpGet("my")]
		[AuthorizeRoles(UserRole.Admin, UserRole.Member)]
		public async Task<ActionResult<List<BorrowingBasicDto>>> GetMy([FromQuery] BorrowingStatus? status, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBorrowingsByUserIdQuery(userContext.UserId, status), cancellationToken));

		[HttpGet("user/{userId:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingBasicDto>>> GetByUserId([FromRoute] int userId, [FromQuery] BorrowingStatus? status, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBorrowingsByUserIdQuery(userId, status), cancellationToken));

        [HttpGet("active")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<ActionResult<List<BorrowingDetailDto>>> GetAllActive(CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetAllActiveBorrowingsQuery(), cancellationToken));

        [HttpGet("overdue")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetAllOverdue(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetOverdueBorrowingsQuery(), cancellationToken));

		[HttpPost("borrow")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Borrow([FromBody] BorrowBookCommand command, CancellationToken cancellationToken)
		{
			await mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpPut("{borrowingId:int}/return")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Return([FromRoute] int borrowingId, CancellationToken cancellationToken)
		{
			await mediator.Send(new ReturnBookCommand(borrowingId), cancellationToken);

			return NoContent();
		}
	}
}
