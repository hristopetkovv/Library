namespace Library.Controllers.Chat
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        [HttpPost]
        [AuthorizeRoles(UserRole.Admin, UserRole.Member)]
        public async Task<ActionResult<ChatResponseDto>> Chat([FromBody] ChatRequestDto request, CancellationToken cancellationToken)
        {
            var response = await chatService.SendMessageAsync(request.Message, request.Language, cancellationToken);

            return Ok(new ChatResponseDto(response));
        }
    }
}
