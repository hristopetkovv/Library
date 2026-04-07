namespace Library.Application.Publishers.Commands.DeletePublisher
{
	public class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommand>
	{
		public DeletePublisherCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid Publisher ID is required");
		}
	}
}
