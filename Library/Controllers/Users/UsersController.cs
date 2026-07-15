using Library.Application.Users.Commands.ActivateUser;
using Library.Application.Users.Commands.DeactivateUser;

namespace Library.Controllers.Users
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<ActionResult<List<UserListDto>>> GetAll([FromQuery] SearchUsersFilterDto? filter, CancellationToken cancellationToken)
			=> Ok(await mediator.Send(new GetAllUsersQuery(filter), cancellationToken));

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

        [HttpPut("{id:int}/activate")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> Activate([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new ActivateUserCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}/deactivate")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> Deactivate([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeactivateUserCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
		[AuthorizeRoles(UserRole.Admin)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{
			await mediator.Send(new DeleteUserCommand(id), cancellationToken);

			return NoContent();
		}
	}
}
