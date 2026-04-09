namespace Library.Infrastructure.Helpers.Configuration
{
	public static class AppSettingsProvider
	{
		public static JwtConfiguration JwtConfiguration { get; private set; } = null!;

		public static void AddAppSettingsConfiguration(IConfiguration configuration)
		{
			if (configuration.GetSection("JwtConfiguration").Exists())
			{
				JwtConfiguration = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;
			}
		}
	}
}
