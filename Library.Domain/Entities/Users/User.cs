namespace Library.Domain.Entities.Users
{
	public class User : BaseAuditableEntity
	{
		private readonly List<Borrowing> borrowings = [];
		private const int MaxActiveBorrowings = 5;

		public int Id { get; private set; }
		public string PasswordSalt { get ; private set; } = null!;
		public string PasswordHash { get ; private set; } = null!;
		// Used as username for login, so it must be unique and not null or empty
		public Email Email { get; private set; } = null!;
		public UserRole Role { get; private set; }
		public FullName FullName { get; private set; } = null!;
		public ContactInfo? ContactInfo { get; private set; }

		public IReadOnlyList<Borrowing> Borrowings => borrowings.AsReadOnly();

		public static User Create(string passwordSalt, string passwordHash, Email email, UserRole role, FullName fullName, ContactInfo? contactInfo)
		{
			return new User
			{
				PasswordSalt = passwordSalt,
				PasswordHash = passwordHash,
				Email = email,
				Role = role,
				FullName = fullName,
				ContactInfo = contactInfo
			};
		}

		public bool CanBorrow()
		{
			var activeBorrowings = borrowings.Count(b => b.Status == BorrowingStatus.Borrowed);

			return activeBorrowings < MaxActiveBorrowings;
		}

		public bool HasOverdueBooks() => borrowings.Any(b => b.IsOverdue());

		public int GetActiveBorrowingsCount() => borrowings.Count(b => b.Status == BorrowingStatus.Borrowed);
	}
}
