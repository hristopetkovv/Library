namespace Library.Application
{
	public static class DepedencyInjection
	{
		public static void AddApplication(this IServiceCollection services)
		{
			var assembly = Assembly.GetExecutingAssembly();

			services.AddValidatorsFromAssembly(assembly);

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);
				cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
				cfg.AddOpenBehavior(typeof(ValidationCommandBehavior<,>));
			});
		}
	}
}
