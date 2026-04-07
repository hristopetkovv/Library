namespace Library.Infrastructure.Repositories
{
    public class PublisherRepository(LibraryDbContext context) : Repository<Publisher>(context), IPublisherRepository
    {
        public async Task<Publisher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Name == name, cancellationToken);

		public async Task<List<Publisher>> GetAllWithBooksAsync(CancellationToken cancellationToken = default)
			=> await dbSet.AsNoTracking().Include(a => a.Books).ToListAsync(cancellationToken);
	}
}
