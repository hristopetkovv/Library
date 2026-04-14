namespace Library.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
	{
        private readonly LibraryDbContext context;
        private IDbContextTransaction? transaction;

        public UnitOfWork(
            LibraryDbContext context,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IBorrowingRepository borrowingRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository
            )
        {
            this.context = context;
            Books = bookRepository;
            Users = userRepository;
            Borrowings = borrowingRepository;
            Authors = authorRepository;
            Publishers = publisherRepository;
        }

        public IBookRepository Books { get; }

        public IUserRepository Users { get; }

        public IBorrowingRepository Borrowings { get; }

        public IAuthorRepository Authors { get; }

        public IPublisherRepository Publishers { get; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
            => transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
			if (transaction == null)
				throw new InvalidOperationException("Transaction not started");

			try
            {
                if (transaction is not null)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
			if (transaction == null)
				throw new InvalidOperationException("Transaction has not been started.");

			await transaction?.RollbackAsync(cancellationToken)!;

            transaction?.Dispose();
            transaction = null;
        }

        public void Dispose()
        {
            transaction?.Dispose();
            context.Dispose();
        }
    }
}
