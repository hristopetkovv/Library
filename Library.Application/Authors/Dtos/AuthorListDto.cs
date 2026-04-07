namespace Library.Application.Authors.Dtos
{
	public record AuthorListDto(
		int Id,
		string Name,
		int BooksCount
	);
}
