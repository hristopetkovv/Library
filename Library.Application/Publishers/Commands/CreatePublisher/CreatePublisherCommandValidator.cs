namespace Library.Application.Publishers.Commands.CreatePublisher
{
	public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
	{
		public CreatePublisherCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Publisher name is required")
				.MaximumLength(200).WithMessage("Publisher name cannot exceed 200 characters");
		}
	}
}
