namespace Library.Infrastructure.Services.Users
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public int GetUserId()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        public string? GetUserEmail() => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
    }
}
