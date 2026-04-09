namespace Library.Application.Authors.Commands.DeleteAuthor
{
	public class DeleteAuthorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAuthorCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(command.Id, cancellationToken, a => a.Books);
			if (author == null)
				throw new NotFoundException(nameof(Author), command.Id);

			if (author.Books.Any())
				throw new InvalidOperationException("Cannot delete an author that has associated books");

			unitOfWork.Authors.Remove(author);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
