namespace Library.Application.Publishers.Dtos
{
	public record PublisherDetailDto(
		int Id,
		string Name,
		List<BookListDto> Books
	);
}
