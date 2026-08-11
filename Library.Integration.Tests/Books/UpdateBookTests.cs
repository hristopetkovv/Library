namespace Library.Integration.Tests.Books
{
    public class UpdateBookTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task UpdateBook_ShouldReturnOk_WhenValid()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre1 = await SeedGenreAsync();
            var genre2 = await SeedGenreAsync("Mystery", "Мистерия", Category.Fiction);
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre1.Id]);

            var request = new
            {
                title = "The Hobbit: Updated",
                authorId = author.Id,
                publisherId = publisher.Id,
                isbn = "9780261102217",
                description = "Updated description",
                pages = 320,
                language = 2,
                coverType = 2,
                publicationYear = 2024,
                totalCopies = 8,
                availableCopies = 6,
                genreIds = new[] { genre2.Id }
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/books/{book.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var updated = await db.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == book.Id);
            updated.Should().NotBeNull();
            updated!.Title.Should().Be("The Hobbit: Updated");
            updated.ISBN.Value.Should().Be("9780261102217");
            updated.TotalCopies.Should().Be(8);
            updated.AvailableCopies.Should().Be(6);
            updated.Genres.Should().ContainSingle(g => g.GenreId == genre2.Id);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn404_WhenBookNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();

            var request = new
            {
                title = "The Hobbit",
                authorId = author.Id,
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
            var response = await Client.PutAsJsonAsync("/api/books/9999", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn404_WhenAuthorNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);

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
                genreIds = new[] { genre.Id }
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/books/{book.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn404_WhenPublisherNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);

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
                genreIds = new[] { genre.Id }
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/books/{book.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn400_WhenValidationFails()
        {
            // Arrange
            AuthenticateAsAdmin();
            var request = new
            {
                title = "The Hobbit",
                authorId = 1,
                publisherId = 1,
                isbn = "123",
                pages = 310,
                language = 1,
                coverType = 1,
                publicationYear = 1937,
                totalCopies = 5,
                availableCopies = 5,
                genreIds = new[] { 1 }
            };

            // Act
            var response = await Client.PutAsJsonAsync("/api/books/1", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn401_WhenNotAuthenticated()
        {
            // Arrange
            var request = new { title = "The Hobbit" };

            // Act
            var response = await Client.PutAsJsonAsync("/api/books/1", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturn403_WhenMember()
        {
            // Arrange
            AuthenticateAsMember();
            var request = new { title = "The Hobbit" };

            // Act
            var response = await Client.PutAsJsonAsync("/api/books/1", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
