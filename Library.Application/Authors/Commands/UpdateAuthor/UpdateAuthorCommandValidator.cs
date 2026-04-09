namespace Library.Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
	{
		public UpdateAuthorCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid Author ID is required");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Author name is required")
				.MaximumLength(200).WithMessage("Author name cannot exceed 200 characters");

			RuleFor(x => x.Biography)
				.NotEmpty().WithMessage("Biography is required")
				.MaximumLength(2000).WithMessage("Biography cannot exceed 2000 characters");
		}
	}
}
