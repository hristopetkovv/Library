namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository(LibraryDbContext context) : Repository<Author>(context), IAuthorRepository
    {
        public async Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Name == name, cancellationToken);

        public async Task<List<Author>> GetAllWithBooksAsync(CancellationToken cancellationToken = default)
            => await dbSet.AsNoTracking().Include(a => a.Books).ToListAsync(cancellationToken);
	}
}
