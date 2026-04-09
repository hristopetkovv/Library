namespace Library
{
	public static class DependencyInjection
	{
		public static void AddApi(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();

			services.AddJwtAuthentication(configuration);
			services.AddAuthorization();
		}

		private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtConfig = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(e =>
				{
					e.SaveToken = true;
					e.RequireHttpsMetadata = false;
					e.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtConfig!.Issuer,
						ValidAudience = jwtConfig.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
						RequireExpirationTime = true
					};
				});
		}
	}
}
