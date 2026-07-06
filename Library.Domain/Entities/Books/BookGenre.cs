namespace Library.Domain.Entities.Books
{
    public class BookGenre : IEntity
    {
        public int Id { get; private set; }

        public int BookId { get; private set; }
        public Book Book { get; private set; } = null!;

        public int GenreId { get; private set; }
        public Genre Genre { get; private set; } = null!;

        public static BookGenre Create(int genreId)
        {
            return new BookGenre
            {
                GenreId = genreId
            };
        }
    }
}
