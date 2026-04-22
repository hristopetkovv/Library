using Library.Application.Borrowings.Commands.BorrowBook;
using Library.Application.Borrowings.Commands.ReturnBook;
using Library.Application.Borrowings.Queries.GetAllActiveBorrowings;

namespace Library.Controllers.Borrowings
{
	[ApiController]
	[Route("api/[controller]")]
	public class BorrowingsController(IMediator mediator, IUserContext userContext) : ControllerBase
	{
		[HttpGet("my-active")]
		[AuthorizeRoles(UserRole.Admin, UserRole.Member)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetMyActive(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetActiveBorrowingsByUserIdQuery(userContext.UserId), cancellationToken));

		[HttpGet("user/{userId:int}/active")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetActiveByUserId([FromRoute] int userId, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetActiveBorrowingsByUserIdQuery(userId), cancellationToken));

		[HttpGet("my-history")]
		[AuthorizeRoles(UserRole.Admin, UserRole.Member)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetMyHistory(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBorrowingHistoryByUserIdQuery(userContext.UserId), cancellationToken));

		[HttpGet("user/{userId:int}/history")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetHistoryByUserId([FromRoute] int userId, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetBorrowingHistoryByUserIdQuery(userId), cancellationToken));

		[HttpGet("overdue")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetAllOverdue(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetOverdueBorrowingsQuery(), cancellationToken));

		[HttpGet("active")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<BorrowingDetailDto>>> GetAllActive(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllActiveBorrowingsQuery(), cancellationToken));

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
