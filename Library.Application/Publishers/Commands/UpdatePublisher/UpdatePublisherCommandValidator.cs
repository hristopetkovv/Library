namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
	{
		public UpdatePublisherCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid Publisher ID is required");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Publisher name is required")
				.MaximumLength(200).WithMessage("Publisher name cannot exceed 200 characters");
		}
	}
}
