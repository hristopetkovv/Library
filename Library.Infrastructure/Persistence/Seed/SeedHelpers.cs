namespace Library.Infrastructure.Persistence.Seed
{
    public static class SeedHelpers
    {
        public static async Task<User> SeedAdminAsync(LibraryDbContext context, DateTime now)
        {
            var admin = User.Create(
                "tempSalt", "tempHash",
                Email.Create("admin@library.com"),
                UserRole.Admin,
                FullName.Create("Admin", "User"),
                null
            );

            admin.CreatedByUserId = 1;
            admin.CreatedDate = now;

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();

            return admin;
        }

        public static async Task<List<Genre>> SeedGenresAsync(LibraryDbContext context, int adminId, DateTime now)
        {
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
                Genre.Create("Children", "Детски", Category.NonFiction)
            };

            foreach (var g in genres)
            {
                g.CreatedByUserId = adminId;
                g.CreatedDate = now;
            }

            await context.Genres.AddRangeAsync(genres);
            await context.SaveChangesAsync();

            return genres;
        }

        public static async Task<Dictionary<string, Author>> SeedAuthorsAsync(LibraryDbContext context, int adminId, DateTime now)
        {
            var authors = SeedData.Authors.Select(a =>
            {
                var author = Author.Create(a.Name, a.Description);
                author.CreatedByUserId = adminId;
                author.CreatedDate = now;
                return author;
            }).ToList();

            await context.Authors.AddRangeAsync(authors);
            await context.SaveChangesAsync();

            return authors.ToDictionary(a => a.Name);
        }

        public static async Task<Dictionary<string, Publisher>> SeedPublishersAsync(LibraryDbContext context, int adminId, DateTime now)
        {
            var publishers = SeedData.Publishers.Select(p =>
            {
                var publisher = Publisher.Create(p.Name);
                publisher.CreatedByUserId = adminId;
                publisher.CreatedDate = now;
                return publisher;
            }).ToList();

            await context.Publishers.AddRangeAsync(publishers);
            await context.SaveChangesAsync();

            return publishers.ToDictionary(p => p.Name);
        }

        public static async Task SeedBooksAsync(
            LibraryDbContext context,
            Dictionary<string, Author> authors,
            Dictionary<string, Publisher> publishers,
            List<Genre> genres,
            ICoverService coverService,
            int adminId,
            DateTime now
        )
        {
            var books = new List<Book>();

            foreach (var bookData in SeedData.Books)
            {
                if (!authors.TryGetValue(bookData.AuthorName, out var author))
                {
                    continue;
                }

                if (!publishers.TryGetValue(bookData.PublisherName, out var publisher))
                {
                    continue;
                }

                var coverImageUrl = await coverService.TryDownloadCoverAsync(bookData.ISBN);

                var bookGenreIds = genres
                    .Where(g => bookData.GenreNames.Contains(g.Name))
                    .Select(g => g.Id)
                    .ToList();

                var book = Book.Create(
                    bookData.Title,
                    author.Id,
                    publisher.Id,
                    ISBN.Create(bookData.ISBN),
                    bookData.Description,
                    bookData.Pages,
                    bookData.Language,
                    bookData.CoverType,
                    bookData.Year,
                    bookData.TotalCopies,
                    coverImageUrl,
                    bookGenreIds
                );

                book.CreatedByUserId = adminId;
                book.CreatedDate = now;

                books.Add(book);
            }

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }
    }
}
