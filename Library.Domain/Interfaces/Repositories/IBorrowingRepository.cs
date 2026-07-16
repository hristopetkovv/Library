namespace Library.Domain.Interfaces.Repositories
{
    public interface IBorrowingRepository : IRepository<Borrowing>
    {
		Task<List<Borrowing>> GetByUserIdAsync(int userId, BorrowingStatus? status, CancellationToken cancellationToken = default);
		Task<List<Borrowing>> GetBorrowingsAsync(Expression<Func<Borrowing, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
