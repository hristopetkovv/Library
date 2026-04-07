namespace Library.Application.Authors.Dtos
{
	public record AuthorDetailDto(
		int Id,
		string Name,
		string? Biography,
		List<BookBasicDto> Books
	);
}
