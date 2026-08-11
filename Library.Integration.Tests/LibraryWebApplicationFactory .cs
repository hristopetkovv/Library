namespace Library.Integration.Tests
{
    public class LibraryWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("library_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        public async Task InitializeAsync() => await postgreSqlContainer.StartAsync();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["JwtConfiguration:SecretKey"] = "zrQOx1N4x2slt7NmiCJX2g==Qe3RT5wv",
                    ["JwtConfiguration:Issuer"] = "http://localhost:4200",
                    ["JwtConfiguration:Audience"] = "http://localhost:4200",
                    ["ConnectionStrings:DefaultConnection"] = postgreSqlContainer.GetConnectionString()
                });
            });

            builder.ConfigureServices(services =>
            {
                var coverDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICoverService));
                if (coverDescriptor != null) 
                    services.Remove(coverDescriptor);

                services.AddScoped<ICoverService>(_ => Mock.Of<ICoverService>(s =>
                    s.TryDownloadCoverAsync(It.IsAny<string>()) == Task.FromResult<string?>(null)));

                var descDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDescriptionService));
                if (descDescriptor != null) 
                    services.Remove(descDescriptor);

                services.AddScoped<IDescriptionService>(_ => Mock.Of<IDescriptionService>(s =>
                    s.TryGetDescriptionAsync(It.IsAny<string>()) == Task.FromResult<string?>(null)));

                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LibraryDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<LibraryDbContext>((sp, options) =>
                {
                    var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
                    options
                        .UseNpgsql(postgreSqlContainer.GetConnectionString())
                        .UseSnakeCaseNamingConvention()
                        .AddInterceptors(interceptor);
                });

                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidateLifetime = false;
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("zrQOx1N4x2slt7NmiCJX2g==Qe3RT5wv"));
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                db.Database.Migrate();
            });
        }

        public new async Task DisposeAsync() => await postgreSqlContainer.DisposeAsync();
    }
}
