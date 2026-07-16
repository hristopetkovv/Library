namespace Library.Infrastructure.Repositories
{
    public class Repository<T>(LibraryDbContext context) : IRepository<T>
        where T : class, IEntity
    {
        protected readonly DbSet<T> dbSet = context.Set<T>();

		public virtual Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
		{
            IQueryable<T> query = dbSet.AsNoTracking();

			foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public Task<T?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = dbSet;

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
		{
            IQueryable<T> query = dbSet.AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToListAsync(cancellationToken);
		}

		public Task<List<T>> GetAllFilteredAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = dbSet
                .AsNoTracking()
                .Where(predicate);

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return query.ToListAsync(cancellationToken);
		}

		public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

        public Task<T?> FirstOrDefaultUpdateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

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
            => await dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
		{
			if (predicate == null)
			{
				return await context.Set<T>().CountAsync(cancellationToken);
			}

			return await context.Set<T>().CountAsync(predicate, cancellationToken);
		}
	}
}
