namespace Library.Application.Authors.Commands.DeleteAuthor
{
	public class DeleteAuthorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAuthorCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(command.Id, cancellationToken, a => a.Books);
			if (author is null)
				throw new NotFoundException(ValidationMessages.AuthorNotFound);

			if (author.Books.Any())
				throw new BadRequestException(ValidationMessages.AuthorHasAssociatedBooks);

			unitOfWork.Authors.Remove(author);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
