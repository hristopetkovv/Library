namespace Library.Application.Common.Exceptions
{
	public class DomainValidationException : Exception
	{
		public DomainValidationException(string message)
			: base(message)
		{
		}

		public DomainValidationException(IDictionary<string, string[]> errors)
			: base("One or more validation errors occurred.")
		{
			Errors = errors;
		}

		public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
	}
}
