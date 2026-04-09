namespace Library.Application.Dtos.Auth
{
	public record AuthResponse(string Token, UserLoginInfoDto User);
}
