namespace Library.Infrastructure.Repositories
{
    public class BorrowingRepository(LibraryDbContext context) : Repository<Borrowing>(context), IBorrowingRepository
    {
        public async Task<List<Borrowing>> GetByUserIdAsync(int userId, BorrowingStatus? status, CancellationToken cancellationToken = default)
        {
            return await dbSet
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .Where(b => status == null || b.Status == status)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Author)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Publisher)
                .Include(b => b.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Borrowing>> GetBorrowingsAsync(Expression<Func<Borrowing, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbSet
                .AsNoTracking()
                .Where(predicate)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Author)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Publisher)
                .Include(b => b.User)
                .OrderBy(b => b.DueDate)
                .ToListAsync(cancellationToken);
        }
    }
}
