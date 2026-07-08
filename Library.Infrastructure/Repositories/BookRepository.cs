namespace Library.Infrastructure.Repositories
{
    public class BookRepository(LibraryDbContext context) : Repository<Book>(context), IBookRepository
    {
        public override async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Book, object>>[] includes)
        {
            IQueryable<Book> query = dbSet.AsNoTracking()
                .Include(b => b.Genres)
                    .ThenInclude(g => g.Genre);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}
