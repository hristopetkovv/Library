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

			// 1. Seed Genres
			var genres = new List<Genre>
			{
				Genre.Create("World Classics", "Световна класика", Category.Fiction),
				Genre.Create("Contemporary Prose", "Съвременна проза", Category.Fiction),
				Genre.Create("Bulgarian Prose", "Българска проза", Category.Fiction),
				Genre.Create("Thrillers and Crimes", "Трилъри и крими", Category.Fiction),
				Genre.Create("Fantasy", "Фантастика и фентъзи", Category.Fiction),
				Genre.Create("Romance Novels", "Любовни романи", Category.Fiction),
				Genre.Create("Historical Novels", "Исторически романи", Category.Fiction),
				Genre.Create("Poetry and Drama", "Поезия и драматургия", Category.Fiction),
				Genre.Create("Horror", "Ужаси", Category.Fiction),
				Genre.Create("Children", "Детски", Category.Fiction),

				Genre.Create("Psychology", "Психология", Category.NonFiction),
				Genre.Create("History", "История", Category.NonFiction),
				Genre.Create("Biographies", "Биографии", Category.NonFiction),
				Genre.Create("Science and Technology", "Наука и технологии", Category.NonFiction),
				Genre.Create("Health and Sports", "Здраве и спорт", Category.NonFiction),
				Genre.Create("Marketing and Management", "Маркетинг и мениджмънт", Category.NonFiction),
				Genre.Create("Economics and Law", "Икономика и право", Category.NonFiction),
				Genre.Create("Cooking", "Кулинария", Category.NonFiction),
				Genre.Create("Tourism", "Туризъм", Category.NonFiction),
				Genre.Create("Politics and Philosophy", "Политика и философия", Category.NonFiction),
				Genre.Create("Architecture and Design", "Архитектура и дизайн", Category.NonFiction),
				Genre.Create("Art", "Изкуства", Category.NonFiction),
				Genre.Create("Memoir and Autobiography", "Мемоари и автобиография", Category.NonFiction),
				Genre.Create("Humor", "Хумор", Category.NonFiction),
				Genre.Create("Religion", "Религия", Category.NonFiction),
				Genre.Create("Parenting and Family", "Родителство и семейства", Category.NonFiction),
				Genre.Create("Children", "Детски", Category.NonFiction),
			};

			foreach (var g in genres)
			{
				g.CreatedByUserId = 1;
				g.CreatedDate = dateTimeNow;
			}

			await context.Genres.AddRangeAsync(genres);
			await context.SaveChangesAsync();

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
			var lotr = Book.Create("The Lord of the Rings", author1.Id, publisher1.Id, ISBN.Create("9780544003415"), "Epic high fantasy novel", 1178, Language.English, CoverType.Hardcover, 1954, 5);
			lotr.CreatedByUserId = 1;
			lotr.CreatedDate = dateTimeNow;

			lotr.AddGenre(genres.First(g => g.Name == "Fantasy"));
			lotr.AddGenre(genres.First(g => g.Name == "World Classics"));

			var hp = Book.Create("Harry Potter and the Philosopher's Stone", author2.Id, publisher2.Id, ISBN.Create("9780747532699"), "First book in the Harry Potter series", 223, Language.English, CoverType.Softcover, 1997, 3);
			hp.CreatedByUserId = 1;
			hp.CreatedDate = dateTimeNow;

			hp.AddGenre(genres.First(g => g.Name == "Fantasy"));

			await context.Books.AddRangeAsync(lotr, hp);
			await context.SaveChangesAsync();
		}
	}
}
