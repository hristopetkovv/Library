namespace Library.Application.Books.Dtos
{
	public record SearchBooksFilterDto(
		string? Term,
		int? AuthorId,
		int? PublisherId,
		Language? Language,
		CoverType? CoverType,
		int? PublicationYear,
		List<int>? GenreIds,
		bool? AvailableOnly
	)
	{
		public Expression<Func<Book, bool>> Predicate()
		{
			return b =>
			(string.IsNullOrEmpty(Term) || b.Title.Contains(Term) || b.Author.Name.Contains(Term) || b.ISBN.Value.Contains(Term))
			&& (AuthorId == null || b.AuthorId == AuthorId)
			&& (PublisherId == null || b.PublisherId == PublisherId)
			&& (Language == null || b.Language == Language)
			&& (CoverType == null || b.CoverType == CoverType)
			&& (PublicationYear == null || b.PublicationYear == PublicationYear)
			&& (GenreIds == null || !GenreIds.Any() || b.Genres.Any(g => GenreIds.Contains(g.Id)))
			&& (AvailableOnly != true || b.AvailableCopies > 0);
		}
	}
}
