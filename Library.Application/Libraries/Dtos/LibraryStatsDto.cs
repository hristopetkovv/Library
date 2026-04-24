namespace Library.Application.Libraries.Dtos
{
	public record LibraryStatsDto(
		int TotalBooks,
		int TotalAuthors,
		int TotalPublishers
	);
}
