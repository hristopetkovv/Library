namespace Library.Application.Publishers.Commands.CreatePublisher
{
	public record CreatePublisherCommand(string Name) : ICommand<Unit>;
}
