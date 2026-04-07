namespace Library.Application.Publishers.Commands.DeletePublisher
{
	public record DeletePublisherCommand(int Id) : ICommand<Unit>;
}
