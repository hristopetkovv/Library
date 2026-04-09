namespace Library.Infrastructure.Services.Auth
{
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		public string GenerateToken(User user)
		{
			var claims = new List<Claim>
			{
				new (JwtRegisteredClaimNames.Jti, user.Id.ToString()),
				new (JwtRegisteredClaimNames.Email, user.Email.Value),
				new (ClaimTypes.Role, user.Role.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsProvider.JwtConfiguration.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: AppSettingsProvider.JwtConfiguration.Issuer,
				audience: AppSettingsProvider.JwtConfiguration.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(AppSettingsProvider.JwtConfiguration.ValidDays),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
