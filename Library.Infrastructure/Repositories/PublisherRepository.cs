namespace Library.Infrastructure.Repositories
{
    public class PublisherRepository(LibraryDbContext context) : Repository<Publisher>(context), IPublisherRepository
    {
        public async Task<Publisher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await dbSet.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }
}
