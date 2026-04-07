namespace Library.Application.Interfaces.Auth
{
	public interface IUserContext
	{
		int GetUserId();
		string? GetUserEmail();
	}
}
