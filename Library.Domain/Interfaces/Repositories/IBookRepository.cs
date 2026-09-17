namespace Library.Domain.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> SearchByDescriptionAsync(string term, CancellationToken cancellationToken = default);
        Task<List<Review>> GetReviewsAsync(int bookId, CancellationToken cancellationToken = default);
    }
}
