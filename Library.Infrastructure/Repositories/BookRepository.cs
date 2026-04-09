namespace Library.Infrastructure.Repositories
{
    public class BookRepository(LibraryDbContext context) : Repository<Book>(context), IBookRepository
    {
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
