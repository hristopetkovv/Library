namespace Library.Application.Publishers.Commands.CreatePublisher
{
	public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
	{
		public CreatePublisherCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.PublisherNameRequired)
				.MaximumLength(200).WithMessage(ValidationMessages.PublisherNameMaxLength);
		}
	}
}
