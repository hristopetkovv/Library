namespace Library.Domain.Interfaces.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<Author>> GetAllWithBooksAsync(CancellationToken cancellationToken = default);
	}
}
