namespace Library.Infrastructure.Repositories
{
    public class UserRepository(LibraryDbContext context) : Repository<User>(context), IUserRepository
    {
        public async Task<List<UserFavoriteBook>> GetFavoriteBooksAsync(int userId, CancellationToken cancellationToken)
        {
            return await dbSet
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Include(b => b.FavoriteBooks)
                    .ThenInclude(r => r.Book)
                        .ThenInclude(b => b.Author)
                .SelectMany(b => b.FavoriteBooks)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsFavoriteAsync(int userId, int bookId, CancellationToken cancellationToken)
        {
            return await dbSet
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .SelectMany(u => u.FavoriteBooks)
                .AnyAsync(fb => fb.BookId == bookId, cancellationToken);
        }
    }
}
