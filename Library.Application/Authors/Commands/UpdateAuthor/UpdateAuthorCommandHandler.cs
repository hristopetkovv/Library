namespace Library.Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateAuthorCommand, Unit>
	{
		public async Task<Unit> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (author is null)
				throw new NotFoundException(nameof(Author), command.Id);

			var existingAuthor = await unitOfWork.Authors.FirstOrDefaultAsync(a => a.Name == command.Name, cancellationToken);
			if (existingAuthor is not null && existingAuthor.Id != command.Id)
				throw new BadRequestException($"Another author with name '{command.Name}' already exists");

			author.Update(command.Name, command.Biography);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
