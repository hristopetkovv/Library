namespace Library.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpContextAccessor();

			services.AddServices();

			services.AddDbContext(configuration);

			AppSettingsProvider.AddAppSettingsConfiguration(configuration);
		}
		private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddScoped<AuditableEntityInterceptor>();

			services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
			{
				var interceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();

				options
					.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
					.UseSnakeCaseNamingConvention()
					.AddInterceptors(interceptor);
			});
		}

		private static void AddServices(this IServiceCollection services)
		{
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBorrowingRepository, BorrowingRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserContext, UserContext>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IPasswordHasher, PasswordHasher>();
			services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
		}

		public static async Task SeedDatabaseAsync(this IServiceProvider sp)
		{
			using var scope = sp.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
			await ContextExtensions.SeedAsync(context);
		}
	}
}
