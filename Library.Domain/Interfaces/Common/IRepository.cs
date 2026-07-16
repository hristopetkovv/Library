namespace Library.Domain.Interfaces.Common
{
    public interface IRepository<T>
        where T : class, IEntity
    {
		Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllFilteredAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
		Task<T?> FirstOrDefaultUpdateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    }
}
