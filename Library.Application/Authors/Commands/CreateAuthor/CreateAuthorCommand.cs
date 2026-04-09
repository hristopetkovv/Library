namespace Library.Application.Authors.Commands.CreateAuthor
{
	public record CreateAuthorCommand(
		string Name,
		string Biography
	) : ICommand<AuthorDetailDto>;
}
