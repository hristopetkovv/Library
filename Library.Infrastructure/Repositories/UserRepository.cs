namespace Library.Infrastructure.Repositories
{
    public class UserRepository(LibraryDbContext context) : Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) 
            => await dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
            => await dbSet.AsNoTracking().AnyAsync(u => u.Email.Value == email, cancellationToken);
    }
}
