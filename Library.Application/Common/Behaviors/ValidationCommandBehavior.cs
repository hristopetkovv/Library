namespace Library.Application.Common.Behaviors
{
	public class ValidationCommandBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (!validators.Any())
			{
				return await next(cancellationToken);
			}

			var context = new ValidationContext<TRequest>(request);

			var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

			var failures = validationResults
				.Where(r => r.Errors.Count != 0)
				.SelectMany(r => r.Errors)
				.ToList();

			if (failures.Count != 0)
			{
				var errors = failures
					.GroupBy(e => e.PropertyName)
					.ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

				throw new ValidationErrorException(errors);
			}

			return await next(cancellationToken);
		}
	}
}
