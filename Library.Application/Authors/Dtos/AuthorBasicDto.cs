namespace Library.Application.Authors.Dtos
{
	public record AuthorBasicDto(
		int Id,
		string Name,
		string? Biography
	);
}
