namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository(LibraryDbContext context) : Repository<Author>(context), IAuthorRepository
    {
	}
}
