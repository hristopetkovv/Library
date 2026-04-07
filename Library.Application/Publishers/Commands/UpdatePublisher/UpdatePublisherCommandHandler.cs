namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public class UpdatePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePublisherCommand, PublisherDetailDto>
	{
		public async Task<PublisherDetailDto> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.Id, cancellationToken);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), command.Id);

			var existingPublisher = await unitOfWork.Publishers.GetByNameAsync(command.Name, cancellationToken);
			if (existingPublisher != null && existingPublisher.Id != command.Id)
				throw new InvalidOperationException($"Another publisher with name '{command.Name}' already exists");

			publisher.Update(command.Name);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return publisher.Adapt<PublisherDetailDto>();
		}
	}
}
