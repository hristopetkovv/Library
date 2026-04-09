namespace Library.Infrastructure.Services.Auth
{
	public class JwtTokenGenerator(IOptions<JwtConfiguration> jwtOptions) : IJwtTokenGenerator
	{
		private readonly JwtConfiguration jwtConfig = jwtOptions.Value;

		public string GenerateToken(User user)
		{
			var claims = new List<Claim>
			{
				new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new (JwtRegisteredClaimNames.Email, user.Email.Value),
				new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new (ClaimTypes.NameIdentifier, user.Id.ToString()),
				new (ClaimTypes.Email, user.Email.Value),
				new (ClaimTypes.Role, user.Role.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtConfig.Issuer,
				audience: jwtConfig.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(jwtConfig.ValidDays),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
