namespace Library.Integration.Tests.Books
{
    public class CreateBookTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task CreateBook_ShouldReturnOk_WhenValid()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();

            var request = new
            {
                title = "The Hobbit",
                authorId = author.Id,
                publisherId = publisher.Id,
                isbn = "9780261102217",
                description = "A great book",
                pages = 310,
                language = 1,
                coverType = 1,
                publicationYear = 1937,
                totalCopies = 5,
                availableCopies = 5,
                genreIds = new[] { genre.Id }
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/books", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var book = await db.Books.FirstOrDefaultAsync(b => b.ISBN.Value == "9780261102217");
            book.Should().NotBeNull();
            book!.Title.Should().Be("The Hobbit");
        }

        [Fact]
        public async Task CreateBook_ShouldReturn404_WhenAuthorNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var publisher = await SeedPublisherAsync();

            var request = new
            {
                title = "The Hobbit",
                authorId = 9999,
                publisherId = publisher.Id,
                isbn = "9780261102217",
                pages = 310,
                language = 1,
                coverType = 1,
                publicationYear = 1937,
                totalCopies = 5,
                availableCopies = 5,
                genreIds = new[] { 1 }
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/books", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn404_WhenPublisherNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();

            var request = new
            {
                title = "The Hobbit",
                authorId = author.Id,
                publisherId = 9999,
                isbn = "9780261102217",
                pages = 310,
                language = 1,
                coverType = 1,
                publicationYear = 1937,
                totalCopies = 5,
                availableCopies = 5,
                genreIds = new[] { 1 }
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/books", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn401_WhenNotAuthenticated()
        {
            // Arrange
            var request = new { title = "The Hobbit" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/books", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn403_WhenMember()
        {
            // Arrange
            AuthenticateAsMember();
            var request = new { title = "The Hobbit" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/books", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
