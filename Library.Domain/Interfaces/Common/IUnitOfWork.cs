namespace Library.Domain.Interfaces.Common
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IUserRepository Users { get; }
        IBorrowingRepository Borrowings { get; }
        IAuthorRepository Authors { get; }
        IPublisherRepository Publishers { get; }
        IGenreRepository Genres { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
