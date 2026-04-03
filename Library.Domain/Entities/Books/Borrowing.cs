namespace Library.Domain.Entities.Books
{
	public class Borrowing
	{
		public int Id { get; private set; }
		public int BookId { get; private set; }
		public Book Book { get; private set; } = null!;
		public int UserId { get; private set; }
		public User User { get; private set; } = null!;
		public DateTime BorrowDate { get; private set; }
		public DateTime DueDate { get; private set; }
		public DateTime? ReturnDate { get; private set; }
		public BorrowingStatus Status { get; private set; }

		public static Borrowing Create(int bookId, int userId, int borrowPeriodDays = 30)
		{
			var borrowDate = DateTime.UtcNow;
			return new Borrowing
			{
				BookId = bookId,
				UserId = userId,
				BorrowDate = borrowDate,
				DueDate = borrowDate.AddDays(borrowPeriodDays),
				Status = BorrowingStatus.Borrowed
			};
		}

		public void MarkAsReturned()
		{
			if (Status == BorrowingStatus.Returned)
				throw new InvalidOperationException("Book already returned.");

			ReturnDate = DateTime.UtcNow;
			Status = BorrowingStatus.Returned;
		}

		public bool IsOverdue() => Status == BorrowingStatus.Borrowed && DateTime.UtcNow > DueDate;
	}
}
