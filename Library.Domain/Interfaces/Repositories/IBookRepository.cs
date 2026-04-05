namespace Library.Domain.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task<List<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
