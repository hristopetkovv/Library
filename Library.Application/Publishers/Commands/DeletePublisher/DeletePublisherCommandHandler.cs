namespace Library.Application.Publishers.Commands.DeletePublisher
{
	public class DeletePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePublisherCommand, Unit>
	{
		public async Task<Unit> Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.Id, cancellationToken, p => p.Books);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), command.Id);

			if (publisher.Books.Any())
				throw new InvalidOperationException("Cannot delete a publisher that has associated books");

			unitOfWork.Publishers.Remove(publisher);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
