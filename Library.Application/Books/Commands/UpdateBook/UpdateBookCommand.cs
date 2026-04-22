namespace Library.Application.Books.Commands.UpdateBook
{
	public record UpdateBookCommand(
		int Id,
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
		int AvailableCopies
	) : ICommand<BookDetailDto>;
}
