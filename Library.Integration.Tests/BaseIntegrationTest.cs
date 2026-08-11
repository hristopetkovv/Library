namespace Library.Integration.Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<LibraryWebApplicationFactory>, IAsyncLifetime
    {
        protected readonly HttpClient Client;
        protected readonly LibraryWebApplicationFactory factory;

        protected BaseIntegrationTest(LibraryWebApplicationFactory webFactory)
        {
            factory = webFactory;
            Client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            await db.Database.ExecuteSqlRawAsync(@"
                DELETE FROM book_genres;
                DELETE FROM borrowings;
                DELETE FROM books;
                DELETE FROM authors;
                DELETE FROM publishers;
                DELETE FROM genres;
                DELETE FROM users;
            ");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        protected void AuthenticateAsAdmin()
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateToken(1, "Admin"));
        }

        protected void AuthenticateAsMember(int userId = 2)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateToken(userId, "Member"));
        }

        private static string GenerateToken(int userId, string role)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("zrQOx1N4x2slt7NmiCJX2g==Qe3RT5wv"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, $"{role.ToLower()}@test.com"),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:4200",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected async Task<Author> SeedAuthorAsync(string name = "Test Author")
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var author = Author.Create(name, "");
            await db.Authors.AddAsync(author);
            await db.SaveChangesAsync();

            return author;
        }

        protected async Task<Publisher> SeedPublisherAsync(string name = "Test Publisher")
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var publisher = Publisher.Create(name);
            await db.Publishers.AddAsync(publisher);
            await db.SaveChangesAsync();

            return publisher;
        }

        protected async Task<Genre> SeedGenreAsync(string name = "Fiction", string bgName = "Художествена литература", Category category = Category.Fiction)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var genre = Genre.Create(name, bgName, category);
            await db.Genres.AddAsync(genre);
            await db.SaveChangesAsync();

            return genre;
        }

        protected async Task<Book> SeedBookAsync(int authorId, int publisherId, List<int> genreIds,
            string title = "Test Book", string isbn = "9780261102217", int pages = 310,
            Language language = Language.Bulgarian, CoverType coverType = CoverType.Hardcover,
            int publicationYear = 1937, int totalCopies = 5, string? description = null, string? coverImageUrl = null)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var book = Book.Create(
                title,
                authorId,
                publisherId,
                ISBN.Create(isbn),
                description,
                pages,
                language,
                coverType,
                publicationYear,
                totalCopies,
                coverImageUrl,
                genreIds);

            await db.Books.AddAsync(book);
            await db.SaveChangesAsync();

            return book;
        }

        protected async Task<User> SeedUserAsync(string email = "member@test.com", UserRole role = UserRole.Member)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var user = User.Create(
                "salt",
                "hash",
                Email.Create(email),
                role,
                FullName.Create("Test", "User"),
                ContactInfo.Create("Test Address", "0888123456"));

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        protected async Task<Borrowing> SeedBorrowingAsync(int bookId, int userId)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var borrowing = Borrowing.Create(bookId, userId);

            await db.Borrowings.AddAsync(borrowing);
            await db.SaveChangesAsync();

            return borrowing;
        }
    }
}
