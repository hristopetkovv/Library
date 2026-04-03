namespace Library.Domain.Entities.Books
{
	public class Book : BaseAuditableEntity
	{
		private readonly List<Borrowing> borrowings = [];

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

		public IReadOnlyList<Borrowing> Borrowings => borrowings.AsReadOnly();

		public static Book Create(string title, int authorId, int publisherId, ISBN isbn, string? description, int pages, Language language, CoverType coverType, int publicationYear, int totalCopies)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Title cannot be null or empty.", nameof(title));

			if (totalCopies < 0)
				throw new ArgumentException("Total copies cannot be negative.", nameof(totalCopies));

			return new Book
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
				AvailableCopies = totalCopies
			};
		}

		public bool CanBeBorrowed() => AvailableCopies > 0;

		public void DecrementAvailableCopies()
		{
			if (AvailableCopies <= 0)
				throw new InvalidOperationException($"No available copies of '{Title}'");

			AvailableCopies--;
		}

		public void IncrementAvailableCopies()
		{
			if (AvailableCopies >= TotalCopies)
				throw new InvalidOperationException($"Available copies cannot exceed total copies");

			AvailableCopies++;
		}
	}
}
