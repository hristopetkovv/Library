namespace Library.Infrastructure.Persistence.Extensions
{
	public static class ContextExtensions
	{
		public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
			entry.References.Any(r =>
				r.TargetEntry != null &&
				r.TargetEntry.Metadata.IsOwned() &&
				(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

		public static async Task SeedAsync(LibraryDbContext context)
		{
			await context.Database.MigrateAsync();

			// Check if already seeded
			if (await context.Users.AnyAsync())
				return;

			var dateTimeNow = DateTime.UtcNow;

			// Seed Admin User
			var adminEmail = Email.Create("admin@libray.com");
			var adminFullName = FullName.Create("Admin", "User");

			var admin = User.Create("tempSalt", "tempHash", adminEmail, UserRole.Admin, adminFullName, null);
			admin.CreatedByUserId = 1;
			admin.CreatedDate = dateTimeNow;

			await context.Users.AddAsync(admin);

			// Seed Authors
			var author1 = Author.Create("J.R.R. Tolkien", "English writer and philologist");
			author1.CreatedByUserId = 1;
			author1.CreatedDate = dateTimeNow;

			var author2 = Author.Create("J.K. Rowling", "British author and philanthropist");
			author2.CreatedByUserId = 1;
			author2.CreatedDate = dateTimeNow;

			await context.Authors.AddRangeAsync(author1, author2);
			await context.SaveChangesAsync();

			// Seed Publishers
			var publisher1 = Publisher.Create("HarperCollins");
			publisher1.CreatedByUserId = 1;
			publisher1.CreatedDate = dateTimeNow;

			var publisher2 = Publisher.Create("Bloomsbury");
			publisher2.CreatedByUserId = 1;
			publisher2.CreatedDate = dateTimeNow;

			await context.Publishers.AddRangeAsync(publisher1, publisher2);
			await context.SaveChangesAsync();

			// Seed Books
			var book1 = Book.Create("The Lord of the Rings", author1.Id, publisher1.Id, ISBN.Create("9780544003415"), "Epic high fantasy novel", 1178, Language.English, CoverType.Hardcover, 1954, 5);
			book1.CreatedByUserId = 1;
			book1.CreatedDate = dateTimeNow;

			var book2 = Book.Create("Harry Potter and the Philosopher's Stone", author2.Id, publisher2.Id, ISBN.Create("9780747532699"), "First book in the Harry Potter series", 223, Language.English, CoverType.Softcover, 1997, 3);
			book2.CreatedByUserId = 1;
			book2.CreatedDate = dateTimeNow;

			await context.Books.AddRangeAsync(book1, book2);
			await context.SaveChangesAsync();
		}
	}
}
