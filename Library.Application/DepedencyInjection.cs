namespace Library.Application
{
	public static class DepedencyInjection
	{
		public static void AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(typeof(DepedencyInjection).Assembly);
				cfg.AddBehavior(typeof(ValidateCommandBehavior<,>));
			});
		}
	}
}
