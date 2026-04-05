namespace Library.Domain.Interfaces.Repositories
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        Task<Publisher?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
