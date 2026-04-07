namespace Library.Application.Books.Dtos
{
	public record BookBasicDto (
		int Id,
		string Title,
		string ISBN,
		int AvailableCopies
	);
}
