namespace Library.Application.Publishers.Commands.DeletePublisher
{
	public class DeletePublisherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePublisherCommand, Unit>
	{
		public async Task<Unit> Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdAsync(command.Id, cancellationToken);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), command.Id);

			var hasBooks = await unitOfWork.Books.AnyAsync(b => b.PublisherId == command.Id, cancellationToken);
			if (hasBooks)
				throw new InvalidOperationException("Cannot delete a publisher that has associated books");

			unitOfWork.Publishers.Remove(publisher);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
