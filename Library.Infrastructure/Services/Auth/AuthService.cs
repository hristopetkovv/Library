namespace Library.Infrastructure.Services.Auth
{
	public class AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt) : IAuthService
	{
		public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
		{
			await new LoginRequestValidator().ValidateAndThrowAsync(request, cancellationToken);

			// TODO: implement User status and failed logic count for better security and user experience
			var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken);

			if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
				throw new UnauthorizedAccessException("Invalid email or password.");

			return new AuthResponse(jwt.GenerateToken(user), new UserLoginInfoDto(user.FullName.FirstName, user.FullName.LastName, user.Email.Value, user.Role));
		}

		public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
		{
			await new RegisterRequestValidator().ValidateAndThrowAsync(request, cancellationToken);

			var isEmailExist = await unitOfWork.Users.AnyAsync(u => u.Email.Value == request.Email, cancellationToken);
			if (isEmailExist)
				throw new InvalidOperationException("Email already exists.");

			var passwordHash = passwordHasher.HashPassword(request.Password, out var passwordSalt);

			var user = User.Create(
				passwordSalt,
				passwordHash,
				Email.Create(request.Email),
				UserRole.Member,
				FullName.Create(request.FirstName, request.LastName),
				!string.IsNullOrEmpty(request.Address) && !string.IsNullOrEmpty(request.PhoneNumber)
					? ContactInfo.Create(request.Address, request.PhoneNumber)
					: null);

			await unitOfWork.Users.AddAsync(user, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}
