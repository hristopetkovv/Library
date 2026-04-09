namespace Library.Application.Publishers.Commands.UpdatePublisher
{
	public record UpdatePublisherCommand(
		int Id,
		string Name
	) : ICommand<Unit>;
}
