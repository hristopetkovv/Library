namespace Library.Infrastructure.Middlewares
{
	public class RequestLoggingMiddleware(RequestDelegate next)
	{
		public async Task InvokeAsync(HttpContext context, IUserContext userContext)
		{
			using (LogContext.PushProperty("UserId", userContext.UserId.ToString() ?? "Anonymous"))
			using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
			using (LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress))
			{
				await next(context);
			}
		}
	}
}
