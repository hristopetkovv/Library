namespace Library.Application.Books.Dtos
{
	public record SearchBooksFilterDto(
		string? Term,
		Language? Language,
		CoverType? CoverType,
		List<int>? GenreIds,
		bool? AvailableOnly
	)
	{
		public Expression<Func<Book, bool>> Predicate()
		{
			return b =>
			(string.IsNullOrEmpty(Term) || b.Title.ToLower().Contains(Term.ToLower()) || b.Author.Name.ToLower().Contains(Term) || b.ISBN.Value.Contains(Term))
			&& (Language == null || b.Language == Language)
			&& (CoverType == null || b.CoverType == CoverType)
			&& (GenreIds == null || !GenreIds.Any() || b.Genres.Any(g => GenreIds.Contains(g.Id)))
			&& (AvailableOnly != true || b.AvailableCopies > 0);
		}
	}
}
