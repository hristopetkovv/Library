namespace Library.Application.Authors.Commands.DeleteAuthor
{
	public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
	{
		public DeleteAuthorCommandValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThan(0).WithMessage("Valid Author ID is required");
		}
	}
}
