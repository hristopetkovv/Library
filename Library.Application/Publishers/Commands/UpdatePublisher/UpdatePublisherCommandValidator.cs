namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
	{
		public UpdatePublisherCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage(ValidationMessages.PublisherInvalidId);

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.PublisherNameRequired)
				.MaximumLength(200).WithMessage(ValidationMessages.PublisherNameMaxLength);
		}
	}
}
