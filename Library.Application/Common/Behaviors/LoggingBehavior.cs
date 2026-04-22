namespace Library.Application.Common.Behaviors
{
	public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var requestName = typeof(TRequest).Name;

			logger.LogInformation("Starting request {RequestName} with data: {@RequestData}", requestName, request);

			var timer = Stopwatch.StartNew();

			try
			{
				var response = await next(cancellationToken);

				timer.Stop();

				logger.LogInformation("Finished request {RequestName} in {ElapsedMilliseconds} ms with response: {@ResponseData}", 
					requestName, timer.ElapsedMilliseconds, response);

				return response;
			}
			catch (Exception ex)
			{
				timer.Stop();

				logger.LogError(ex, "Request {RequestName} failed after {ElapsedMilliseconds} ms with error: {ErrorMessage}", 
					requestName, timer.ElapsedMilliseconds, ex.Message);

				throw;
			}
		}
	}
}
