namespace Library.Application.Authors.Commands.DeleteAuthor
{
	public record DeleteAuthorCommand(int Id) : ICommand<Unit>;
}
