namespace Library.Infrastructure.Repositories
{
    public class PublisherRepository(LibraryDbContext context) : Repository<Publisher>(context), IPublisherRepository
    {
	}
}
