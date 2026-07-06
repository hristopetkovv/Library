namespace Library.Domain.Entities.Books
{
	public class Book : BaseAuditableEntity, IEntity
	{
		private readonly List<Borrowing> borrowings = [];
		private readonly List<BookGenre> genres = [];

        public int Id { get; private set; }
		public string Title { get; private set; } = null!;
		public int AuthorId { get; private set; }
		public Author Author { get; private set; } = null!;
		public int PublisherId { get; private set; }
		public Publisher Publisher { get; private set; } = null!;
		public ISBN ISBN { get; private set; } = null!;
		public string? Description { get; private set; }
		public int Pages { get; private set; }
		public Language Language { get; private set; }
		public CoverType CoverType { get; private set; }
		public int PublicationYear { get; private set; }
		public int TotalCopies { get; private set; }
		public int AvailableCopies { get; private set; }
		public string? CoverImageUrl { get; private set; }

		public IReadOnlyList<Borrowing> Borrowings => borrowings.AsReadOnly();
		public IReadOnlyList<BookGenre> Genres => genres.AsReadOnly();

		public static Book Create(string title, int authorId, int publisherId, ISBN isbn, string? description, int pages, Language language, CoverType coverType, int publicationYear, int totalCopies, string? coverImageUrl, List<int> genreIds)
		{
            var book = new Book
            {
                Title = title,
                AuthorId = authorId,
                PublisherId = publisherId,
                ISBN = isbn,
                Description = description,
                Pages = pages,
                Language = language,
                CoverType = coverType,
                PublicationYear = publicationYear,
                TotalCopies = totalCopies,
                AvailableCopies = totalCopies,
                CoverImageUrl = coverImageUrl
            };

			foreach (var genreId in genreIds)
			{
                book.AddGenre(genreId);
            }

			return book;
		}

		public void Update(string title, int authorId, int publisherId, ISBN isbn, string? description, int pages, Language language, CoverType coverType, int publicationYear, int totalCopies, int availableCopies, string? coverImageUrl, List<int> genreIds)
		{
			Title = title;
			AuthorId = authorId;
			PublisherId = publisherId;
			ISBN = isbn;
			Description = description;
			Pages = pages;
			Language = language;
			CoverType = coverType;
			PublicationYear = publicationYear;
			TotalCopies = totalCopies;
			AvailableCopies = availableCopies;
			CoverImageUrl = coverImageUrl;

            var genresToRemove = genres.Where(g => !genreIds.Contains(g.GenreId)).ToList();
            foreach (var genreToRemove in genresToRemove)
            {
                genres.Remove(genreToRemove);
            }

            foreach (var genreId in genreIds)
            {
                AddGenre(genreId);
            }
        }

		public void AddGenre(int genreId)
		{
			if (!Genres.Any(g => g.GenreId == genreId))
				genres.Add(BookGenre.Create(genreId));
		}

		public bool CanBeBorrowed() => AvailableCopies > 0;

		public void DecrementAvailableCopies()
		{
			if (AvailableCopies <= 0)
				throw new DomainException(ValidationMessages.BookHasNoAvailableCopies);

			AvailableCopies--;
		}

		public void IncrementAvailableCopies()
		{
			if (AvailableCopies >= TotalCopies)
				throw new DomainException(ValidationMessages.BookAvailableCannotExceedTotalCopies);

			AvailableCopies++;
		}
	}
}
