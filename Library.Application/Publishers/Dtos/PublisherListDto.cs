namespace Library.Application.Publishers.Dtos
{
	public record PublisherListDto(
		int Id,
		string Name,
		int BooksCount
	);
}
