namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public class UpdatePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePublisherCommand, Unit>
	{
		public async Task<Unit> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), command.Id);

			var existingPublisher = await unitOfWork.Publishers.FirstOrDefaultAsync(p => p.Name == command.Name, cancellationToken);
			if (existingPublisher != null && existingPublisher.Id != command.Id)
				throw new InvalidOperationException($"Another publisher with name '{command.Name}' already exists");

			publisher.Update(command.Name);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
