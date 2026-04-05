namespace Library.Domain.Interfaces.Repositories
{
    public interface IBorrowingRepository : IRepository<Borrowing>
    {
        Task<List<Borrowing>> GetActiveBorrowingsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<Borrowing>> GetOverdueBorrowingsAsync(CancellationToken cancellationToken = default);
        Task<List<Borrowing>> GetBorrowingHistoryByUserAsync(int userId, CancellationToken cancellationToken = default);
    }
}
