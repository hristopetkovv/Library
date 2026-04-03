namespace Library.Infrastructure.Persistence
{
	public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Book> Books => Set<Book>();
		public DbSet<Author> Authors => Set<Author>();
		public DbSet<Publisher> Publishers => Set<Publisher>();
		public DbSet<Borrowing> Borrowings => Set<Borrowing>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys().Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}
	}
}
