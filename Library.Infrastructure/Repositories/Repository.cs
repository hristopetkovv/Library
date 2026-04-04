namespace Library.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly LibraryContext context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LibraryContext context)
        {
            this.context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
            => await _dbSet.FindAsync([id], cancellationToken);

        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) 
            => _dbSet.ToListAsync(cancellationToken);

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default) 
            => await _dbSet.AddAsync(entity, cancellationToken);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) 
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        public void Update(T entity) 
            => _dbSet.Update(entity);

        public void Remove(T entity) 
            => _dbSet.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await _dbSet.AnyAsync(predicate, cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) 
            => await _dbSet.CountAsync(predicate, cancellationToken);
    }
}
