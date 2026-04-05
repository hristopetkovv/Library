namespace Library.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext(configuration);
			services.AddServices();
			services.AddHttpContextAccessor();

			return services;
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
        }
    }
}
