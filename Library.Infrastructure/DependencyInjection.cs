namespace Library.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpContextAccessor();

			services.AddConfiguration(configuration);

			services.AddServices();

			services.AddDbContext(configuration);
		}
		private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddScoped<AuditableEntityInterceptor>();

			services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
			{
				var interceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();

				options
					.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), e => e.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
					.UseSnakeCaseNamingConvention()
					.AddInterceptors(interceptor);
			});
		}

		private static void AddServices(this IServiceCollection services)
		{
			services.AddScoped<IFileStorageService, LocalFileStorageService>();

            services.Scan(scan => scan
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())

                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .AsMatchingInterface()
                .WithScopedLifetime()

                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                .AsMatchingInterface()
                .WithSingletonLifetime()
            );
        }

		public static async Task SeedDatabaseAsync(this IServiceProvider sp)
		{
			using var scope = sp.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var coverService = scope.ServiceProvider.GetRequiredService<ICoverService>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<LibraryDbContext>>();

            await ContextExtensions.SeedAsync(context, coverService, logger);
		}

		private static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.SectionName))
				.Configure<ExternalServicesConfiguration>(configuration.GetSection(ExternalServicesConfiguration.SectionName));
        }
	}
}
