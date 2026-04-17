namespace Library.Infrastructure.Handlers
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			(string Detail, string Title, int statusCode, IDictionary<string, string[]>? errors) = CreateProblemDetails(httpContext, exception);

			var problemDetails = new ProblemDetails
			{
				Detail = Detail,
				Title = Title,
				Status = statusCode,
				Instance = httpContext.Request.Path
			};

			if (errors != null)
			{
				problemDetails.Extensions["errors"] = errors;
			}

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}

		private (string Detail, string Title, int statusCode, IDictionary<string, string[]>? Errors) CreateProblemDetails(HttpContext context, Exception exception)
		{
			return exception switch
			{
				NotFoundException => (exception.Message, "Not Found", context.Response.StatusCode = StatusCodes.Status404NotFound, null),
				ForbiddenException => (exception.Message, "Forbidden", context.Response.StatusCode = StatusCodes.Status403Forbidden, null),
				BadRequestException => (exception.Message, "Bad Request", context.Response.StatusCode = StatusCodes.Status400BadRequest, null),
				UnauthorizedException => (exception.Message, "Unauthorized", context.Response.StatusCode = StatusCodes.Status401Unauthorized, null),
				DomainException => (exception.Message, "Domain Error", context.Response.StatusCode = StatusCodes.Status400BadRequest, null),
				ValidationErrorException validationException => ("One or more validation failures have occurred.", "Validation Error", context.Response.StatusCode = StatusCodes.Status400BadRequest, validationException.Errors),
				_ => (exception.Message, exception.GetType().Name, context.Response.StatusCode = StatusCodes.Status500InternalServerError, null)
			};
		}
	}
}
