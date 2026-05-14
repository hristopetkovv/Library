namespace Library.Application.Books.Dtos
{
	public record BookDetailDto(
		int Id,
		string Title,
		AuthorBasicDto Author,
		PublisherBasicDto Publisher,
		string ISBN,
		string? Description,
		int Pages,
		Language Language,
		CoverType CoverType,
		int PublicationYear,
		int TotalCopies,
		int AvailableCopies,
		List<GenreDto> Genres,
		string? CoverImageUrl
	);
}
