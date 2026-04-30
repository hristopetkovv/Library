namespace Library.Contracts.Books
{
	public record UpdateBookRequest(
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
		List<int> GenreIds,
		IFormFile? CoverImage
	);
}
