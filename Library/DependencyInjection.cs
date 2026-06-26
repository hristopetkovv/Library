namespace Library
{
    public static class DependencyInjection
    {
        public static void AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApi();

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddJwtAuthentication(configuration);

            services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

            services.AddHttpClient("OpenLibrary", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        }

        private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                        ValidIssuer = jwtConfig!.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                        RequireExpirationTime = true
                    };
                });
        }

        private static void AddOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= (IDictionary<string, IOpenApiSecurityScheme>)new Dictionary<string, OpenApiSecurityScheme>();

                    document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter your JWT token"
                    });

                    document.Security ??= [];
                    document.Security.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecuritySchemeReference("Bearer"),
                            new List<string>()
                        }
                    });

                    return Task.CompletedTask;
                });
            });
        }
    }
}
