namespace Library.Domain.Entities.Books
{
    public class UserFavoriteBook
    {
        public int Id { get; private set; }

        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public int BookId { get; private set; }
        public Book Book { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }

        public static UserFavoriteBook Create(int userId, int bookId)
        {
            return new UserFavoriteBook
            {
                UserId = userId,
                BookId = bookId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
