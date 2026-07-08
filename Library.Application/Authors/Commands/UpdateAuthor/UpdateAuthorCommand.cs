namespace Library.Application.Authors.Commands.UpdateAuthor
{
	public record UpdateAuthorCommand(
		int Id,
		string Name,
		string Biography
	) : ICommand<Unit>;
}
