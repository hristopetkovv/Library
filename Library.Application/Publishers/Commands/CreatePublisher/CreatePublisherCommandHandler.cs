namespace Library.Application.Publishers.Commands.CreatePublisher
{
	public class CreatePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePublisherCommand, Unit>
	{
		public async Task<Unit> Handle(CreatePublisherCommand command, CancellationToken cancellationToken)
		{
			var existingPublisher = await unitOfWork.Publishers.AnyAsync(p => p.Name == command.Name, cancellationToken);
			if (existingPublisher)
				throw new BadRequestException(ValidationMessages.PublisherWithThatNameExists);

			var publisher = Publisher.Create(command.Name);

			await unitOfWork.Publishers.AddAsync(publisher, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
