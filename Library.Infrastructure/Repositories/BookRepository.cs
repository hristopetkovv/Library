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
            => await dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.ISBN.Value == isbn, cancellationToken);

        public Task<List<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return dbSet
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Where(b => EF.Functions.ILike(b.Title, $"%{searchTerm}%") ||
							EF.Functions.ILike(b.Author.Name, $"%{searchTerm}%") ||
							EF.Functions.ILike(b.ISBN.Value, $"%{searchTerm}%"))
                .ToListAsync(cancellationToken);
        }
    }
}
