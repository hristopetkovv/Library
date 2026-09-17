namespace Library.Domain.Entities.Books
{
    public class Review : IEntity
    {
        public int Id { get; private set; }
        public string Content { get; private set; } = null!;
        public int Rating { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int BookId { get; private set; }
        public Book Book { get; private set; } = null!;
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public static Review Create(int bookId, int userId, string content, int rating)
        {
            return new Review
            {
                BookId = bookId,
                UserId = userId,
                Content = content,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
