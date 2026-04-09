namespace Library.Infrastructure.Repositories
{
    public class UserRepository(LibraryDbContext context) : Repository<User>(context), IUserRepository
    {
    }
}
