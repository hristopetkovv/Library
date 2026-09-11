namespace Library.Application.Interfaces.Auth
{
	public interface IUserContext : IScopedService
    {
		int UserId { get; }
		UserRole Role { get; }
	}
}
