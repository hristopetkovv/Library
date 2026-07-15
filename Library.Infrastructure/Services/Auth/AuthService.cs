namespace Library.Infrastructure.Services.Auth
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IPasswordHasher passwordHasher;
		private readonly IJwtTokenGenerator jwt;
		private readonly IValidator<LoginRequest> loginValidator;
		private readonly IValidator<RegisterRequest> registerValidator;
		private readonly ILogger<AuthService> logger;

		public AuthService(
			IUnitOfWork unitOfWork, 
			IPasswordHasher passwordHasher, 
			IJwtTokenGenerator jwt,
			IValidator<LoginRequest> loginValidator,
			IValidator<RegisterRequest> registerValidator,
			ILogger<AuthService> logger
			)
		{
			this.unitOfWork = unitOfWork;
			this.passwordHasher = passwordHasher;
			this.jwt = jwt;
			this.loginValidator = loginValidator;
			this.registerValidator = registerValidator;
			this.logger = logger;
		}

		public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
		{
			await ValidateAsync(loginValidator, request, cancellationToken);

			var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken);

			if (user is null)
			{
				logger.LogWarning("Failed login attempt for email: {Email}", request.Email);

				throw new UnauthorizedException(ValidationMessages.UserEmailOrPasswordInvalid);
			}

            if (user.Status == UserStatus.Locked)
                throw new UnauthorizedException(ValidationMessages.UserAccountLocked);

            if (user.Status == UserStatus.Inactive)
                throw new UnauthorizedException(ValidationMessages.UserAccountInactive);

			if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
			{
				user.RecordFailedLogin();

				await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogWarning("Failed login attempt for email: {Email}", request.Email);

                throw new UnauthorizedException(ValidationMessages.UserEmailOrPasswordInvalid);
            }

			user.RecordSuccessfulLogin();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {Email} logged in successfully", request.Email);

			return new AuthResponse(jwt.GenerateToken(user), new UserLoginInfoDto(user.Id, user.FullName.FirstName, user.FullName.LastName, user.Email.Value, user.Role));
		}

		public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
		{
			await ValidateAsync(registerValidator, request, cancellationToken);

			var isEmailExist = await unitOfWork.Users.AnyAsync(u => u.Email.Value == request.Email, cancellationToken);
			if (isEmailExist)
			{
				logger.LogWarning("Attempt to register with existing email: {Email}", request.Email);

				throw new BadRequestException(ValidationMessages.UserEmailExists);
			}

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

			logger.LogInformation("User {Email} registered successfully", request.Email);
		}

		private static async Task ValidateAsync<TRequest>(IValidator<TRequest> validator, TRequest request, CancellationToken ct)
		{
			var result = await validator.ValidateAsync(request, ct);

			if (!result.IsValid)
			{
				throw new ValidationErrorException(result.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray()
					));
			}
		}
	}
}
