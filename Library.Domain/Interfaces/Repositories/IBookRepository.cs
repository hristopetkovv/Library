namespace Library.Domain.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
