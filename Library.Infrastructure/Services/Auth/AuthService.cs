namespace Library.Infrastructure.Services.Auth
{
	public class AuthService(IUnitOfWork unitOfWork) : IAuthService
	{
        public Task<AuthResponse> Login(LoginRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task Register(RegisterRequest request)
		{
            // TODO : Validate request data (e.g., email format, password strength)
            var isEmailExist = await unitOfWork.Users.EmailExistsAsync(request.Email);
			if (isEmailExist)
				throw new InvalidOperationException("Email already exists.");

			var passwordHash = PasswordHelper.HashPassword(request.Password, out var salt);

			var user = User.Create(
				passwordHash, 
				salt, 
				Email.Create(request.Email), 
				UserRole.Member, 
				FullName.Create(request.FirstName, request.LastName),
				ContactInfo.Create(request.Address ?? string.Empty, request.PhoneNumber ?? string.Empty));

			await unitOfWork.Users.AddAsync(user);
			await unitOfWork.SaveChangesAsync();
        }
	}
}
