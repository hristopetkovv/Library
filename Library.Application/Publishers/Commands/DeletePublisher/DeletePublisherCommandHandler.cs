namespace Library.Application.Publishers.Commands.DeletePublisher
{
	public class DeletePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePublisherCommand, Unit>
	{
		public async Task<Unit> Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.Id, cancellationToken, p => p.Books);
			if (publisher is null)
				throw new NotFoundException(ValidationMessages.PublisherNotFound);

			if (publisher.Books.Any())
				throw new BadRequestException(ValidationMessages.PublisherHasAssociatedBooks);

			unitOfWork.Publishers.Remove(publisher);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
