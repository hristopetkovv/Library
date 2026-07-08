namespace Library.Application.Authors.Commands.CreateAuthor
{
	public class CreateAuthorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAuthorCommand, Unit>
	{
		public async Task<Unit> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
		{
			var existingAuthor = await unitOfWork.Authors.AnyAsync(a => a.Name == command.Name, cancellationToken);
			if (existingAuthor)
				throw new BadRequestException(ValidationMessages.AuthorWithThatNameExists);

			var author = Author.Create(command.Name, command.Biography);

			await unitOfWork.Authors.AddAsync(author, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
