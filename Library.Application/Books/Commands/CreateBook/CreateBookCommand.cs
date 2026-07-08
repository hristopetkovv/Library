namespace Library.Application.Books.Commands.CreateBook
{
	public record CreateBookCommand(
		string Title,
		int AuthorId,
		int PublisherId,
		string ISBN,
		string? Description,
		int Pages,
		Language Language,
		CoverType CoverType,
		int PublicationYear,
		int TotalCopies,
		int AvailableCopies,
		List<int> GenreIds
		) : ICommand<Unit>;
}
