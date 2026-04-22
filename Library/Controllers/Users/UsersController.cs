namespace Library.Controllers.Users
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<UserListDto>>> GetAll(CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllUsersQuery(), cancellationToken));

		[HttpGet("{id:int}")]
		[AuthorizeRoles(UserRole.Admin, UserRole.Member)]
		public async Task<ActionResult<UserDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetUserByIdQuery(id), cancellationToken));

		[HttpPut]
		[AuthorizeRoles(UserRole.Admin, UserRole.Member)]
		public async Task<ActionResult<UserDetailDto>> Update([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<UpdateUserCommand>();

			return Ok(await mediator.Send(command, cancellationToken));
		}

		[HttpPut("{id:int}/role")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> ChangeRole([FromRoute] int id, [FromBody] ChangeUserRoleRequest request, CancellationToken cancellationToken)
		{
			var command = request.Adapt<ChangeUserRoleCommand>() with { Id = id };

			return Ok(await mediator.Send(command, cancellationToken));
		}
	}
}
