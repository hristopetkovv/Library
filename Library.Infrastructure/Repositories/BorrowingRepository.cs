namespace Library.Infrastructure.Repositories
{
    public class BorrowingRepository(LibraryDbContext context) : Repository<Borrowing>(context), IBorrowingRepository
    {
        public async Task<List<Borrowing>> GetByUserIdAsync(int userId, BorrowingStatus? status, CancellationToken cancellationToken = default)
        {
            return await dbSet
                .AsNoTracking()
                .Include(b => b.Book)
                    .ThenInclude(book => book.Author)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Publisher)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .Where(b => status == null || b.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Borrowing>> GetAllActiveBorrowingsAsync(CancellationToken cancellationToken = default)
		{
			return await dbSet
				.AsNoTracking()
				.Include(b => b.Book)
					.ThenInclude(book => book.Author)
				.Include(b => b.Book)
					.ThenInclude(book => book.Publisher)
				.Include(b => b.User)
				.Where(b => b.Status == BorrowingStatus.Borrowed)
				.OrderBy(b => b.DueDate)
				.ToListAsync(cancellationToken);
		}

        public async Task<List<Borrowing>> GetOverdueBorrowingsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await dbSet
                .AsNoTracking()
				.Include(b => b.Book)
					.ThenInclude(book => book.Author)
				.Include(b => b.Book)
					.ThenInclude(book => book.Publisher)
				.Include(b => b.User)
                .Where(b => b.Status == BorrowingStatus.Borrowed && b.DueDate < now)
                .ToListAsync(cancellationToken);
        }
    }
}
