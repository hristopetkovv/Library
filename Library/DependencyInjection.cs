namespace Library
{
	public static class DependencyInjection
	{
		public static void AddApi(this IServiceCollection services)
		{
			services.AddControllers();

			services.AddJwtAuthentication();
			services.AddAuthorization();
		}

		private static void AddJwtAuthentication(this IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			})
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
						ValidIssuer = AppSettingsProvider.JwtConfiguration.Issuer,
						ValidAudience = AppSettingsProvider.JwtConfiguration.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsProvider.JwtConfiguration.SecretKey)),
						RequireExpirationTime = true
					};
				});
		}
	}
}
