namespace Library.Application.Interfaces.Auth
{
	public interface IUserContext
	{
		int UserId { get; }
		UserRole Role { get; }
	}
}
