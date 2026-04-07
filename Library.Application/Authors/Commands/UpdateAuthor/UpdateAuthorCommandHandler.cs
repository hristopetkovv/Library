namespace Library.Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateAuthorCommand, AuthorDetailDto>
	{
		public async Task<AuthorDetailDto> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(command.Id, cancellationToken);
			if (author == null)
				throw new NotFoundException(nameof(Author), command.Id);

			var existingAuthor = await unitOfWork.Authors.GetByNameAsync(command.Name, cancellationToken);
			if (existingAuthor != null && existingAuthor.Id != command.Id)
				throw new InvalidOperationException($"Another author with name '{command.Name}' already exists");

			author.Update(command.Name, command.Biography ?? string.Empty);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return author.Adapt<AuthorDetailDto>();
		}
	}
}
