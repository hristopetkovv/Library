namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository(LibraryDbContext context) : Repository<Author>(context), IAuthorRepository
    {
        public async Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await dbSet.FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }
}
