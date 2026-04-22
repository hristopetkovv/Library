namespace Library.Infrastructure.Services.Auth
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private int? userId;
        private string? userEmail;
		private UserRole? userRole;

		public int UserId => userId ??= GetUserId();
		public string? Email => userEmail ??= httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
		public UserRole Role => userRole ??= GetUserRole();

		private int GetUserId()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

		private UserRole GetUserRole()
		{
			var userRole = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            return Enum.TryParse(userRole, out UserRole role) ? role : UserRole.Member;
		}
	}
}
