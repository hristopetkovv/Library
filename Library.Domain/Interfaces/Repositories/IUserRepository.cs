namespace Library.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<UserFavoriteBook>> GetFavoriteBooksAsync(int userId, CancellationToken cancellationToken);
        Task<bool> IsFavoriteAsync(int userId, int bookId, CancellationToken cancellationToken);
    }
}
