namespace Library.Application.Common.Interfaces
{
	public interface IUserContext
	{
		int GetUserId();
		string? GetUserEmail();
	}
}
