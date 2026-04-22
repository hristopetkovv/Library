namespace Library.Infrastructure.Extensions
{
	public static class SerilogExtensions
	{
		public static void AddSerilog(this IHostBuilder host)
		{
			host.UseSerilog((context, loggerConfiguration) =>
			{
				loggerConfiguration.ReadFrom.Configuration(context.Configuration);
			});
		}
	}
}
