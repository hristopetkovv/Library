namespace Library.Infrastructure.Repositories
{
    public class Repository<T>(LibraryDbContext context) : IRepository<T>
        where T : class
    {
        protected readonly DbSet<T> dbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
            => await dbSet.FindAsync([id], cancellationToken);

        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) 
            => dbSet.AsNoTracking().ToListAsync(cancellationToken);

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await dbSet.Where(predicate).ToListAsync(cancellationToken);

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
			await dbSet.AddAsync(entity, cancellationToken);

			return entity;
		}

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) 
            => await dbSet.AddRangeAsync(entities, cancellationToken);

        public void Update(T entity) 
            => dbSet.Update(entity);

        public void Remove(T entity) 
            => dbSet.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await dbSet.AnyAsync(predicate, cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await dbSet.CountAsync(predicate, cancellationToken);
    }
}
