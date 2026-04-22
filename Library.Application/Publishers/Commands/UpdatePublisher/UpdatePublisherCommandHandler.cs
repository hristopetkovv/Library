namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public class UpdatePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePublisherCommand, PublisherDetailDto>
	{
		public async Task<PublisherDetailDto> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdForUpdateAsync(command.Id, cancellationToken);
			if (publisher is null)
				throw new NotFoundException(nameof(Publisher), command.Id);

			var existingPublisher = await unitOfWork.Publishers.FirstOrDefaultAsync(p => p.Name == command.Name, cancellationToken);
			if (existingPublisher is not null && existingPublisher.Id != command.Id)
				throw new BadRequestException($"Another publisher with name '{command.Name}' already exists");

			publisher.Update(command.Name);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return publisher.Adapt<PublisherDetailDto>();
		}
	}
}
