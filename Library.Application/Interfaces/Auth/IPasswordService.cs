namespace Library.Application.Interfaces.Auth
{
	public interface IPasswordService
	{
		string HashPassword(string password, out string salt);
	}
}
