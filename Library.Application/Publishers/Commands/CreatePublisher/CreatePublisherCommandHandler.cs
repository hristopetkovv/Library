namespace Library.Application.Publishers.Commands.CreatePublisher
{
	public class CreatePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePublisherCommand, PublisherDetailDto>
	{
		public async Task<PublisherDetailDto> Handle(CreatePublisherCommand command, CancellationToken cancellationToken)
		{
			var existingPublisher = await unitOfWork.Publishers.GetByNameAsync(command.Name, cancellationToken);
			if (existingPublisher != null)
				throw new InvalidOperationException($"Publisher with name '{command.Name}' already exists");

			var publisher = Publisher.Create(command.Name);

			await unitOfWork.Publishers.AddAsync(publisher, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return publisher.Adapt<PublisherDetailDto>();
		}
	}
}
