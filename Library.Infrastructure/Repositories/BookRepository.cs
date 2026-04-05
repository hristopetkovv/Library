namespace Library.Infrastructure.Repositories
{
    public class BookRepository(LibraryDbContext context) : Repository<Book>(context), IBookRepository
    {
        public async Task<List<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default)
        {
            return await dbSet
                .AsNoTracking()
                .Where(b => b.AvailableCopies > 0)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken = default)
            => await dbSet.FirstOrDefaultAsync(b => b.ISBN.Value == isbn, cancellationToken);

        public Task<List<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            return dbSet
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Where(b => b.Title.ToLower().Contains(lowerSearchTerm) ||
                            b.Author.Name.ToLower().Contains(lowerSearchTerm) ||
                            b.ISBN.Value.Contains(lowerSearchTerm))
                .ToListAsync(cancellationToken);
        }
    }
}
