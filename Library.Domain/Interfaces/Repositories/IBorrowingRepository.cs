namespace Library.Domain.Interfaces.Repositories
{
    public interface IBorrowingRepository : IRepository<Borrowing>
    {
		Task<List<Borrowing>> GetByUserIdAsync(int userId, BorrowingStatus? status, CancellationToken cancellationToken = default);
		Task<List<Borrowing>> GetAllActiveBorrowingsAsync(CancellationToken cancellationToken = default);
        Task<List<Borrowing>> GetOverdueBorrowingsAsync(CancellationToken cancellationToken = default);
    }
}
