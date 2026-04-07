namespace Library.Application.Books.Dtos
{
	public record BookListDto(
		int Id,
		string Title,
		string AuthorName,
		string PublisherName,
		string ISBN,
		int AvailableCopies
	);
}
