namespace Library.Infrastructure.Services.Auth
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IPasswordService passwordService;

		public AuthService(IUnitOfWork unitOfWork, IPasswordService passwordService)
		{
			this.unitOfWork = unitOfWork;
			this.passwordService = passwordService;
		}

		public Task<AuthResponse> Login(LoginRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task Register(RegisterRequest request)
		{
			var isEmailExist = await unitOfWork.Users.EmailExistsAsync(request.Email);
			if (isEmailExist)
				throw new InvalidOperationException("Email already exists.");

			var passwordHash = passwordService.HashPassword(request.Password, out var salt);

			// TODO: CreateUserCommand
		}
	}
}
