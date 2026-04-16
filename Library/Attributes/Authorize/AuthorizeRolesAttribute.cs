namespace Library.Attributes.Authorize
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class AuthorizeRolesAttribute : AuthorizeAttribute
	{
		public AuthorizeRolesAttribute(params UserRole[] roles)
		{
			Roles = string.Join(",", roles);
		}
	}
}
