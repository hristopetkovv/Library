namespace Library.Application.Common.Exceptions
{
	public class ValidationErrorException : Exception
	{
		public ValidationErrorException(IDictionary<string, string[]> errors)
			: base("One or more validation errors occurred.")
		{
			Errors = errors;
		}

		public IDictionary<string, string[]> Errors { get; }
	}
}
