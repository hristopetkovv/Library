namespace Library.Application.Authors.Commands.CreateAuthor
{
	public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
	{
		public CreateAuthorCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Author name is required")
				.MaximumLength(200).WithMessage("Author name cannot exceed 200 characters");

			RuleFor(x => x.Biography)
				.NotEmpty().WithMessage("Biography is required")
				.MaximumLength(2000).WithMessage("Biography cannot exceed 2000 characters");
		}
	}
}
