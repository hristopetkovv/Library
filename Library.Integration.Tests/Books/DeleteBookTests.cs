namespace Library.Integration.Tests.Books
{
    public class DeleteBookTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task DeleteBook_ShouldReturnNoContent_WhenValid()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);

            // Act
            var response = await Client.DeleteAsync($"/api/books/{book.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var deleted = await db.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            deleted.Should().BeNull();

            var bookGenres = await db.BookGenres.CountAsync(bg => bg.BookId == book.Id);
            bookGenres.Should().Be(0);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturn404_WhenBookNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();

            // Act
            var response = await Client.DeleteAsync("/api/books/9999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturn400_WhenBookHasActiveBorrowings()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var user = await SeedUserAsync();
            await SeedBorrowingAsync(book.Id, user.Id);

            // Act
            var response = await Client.DeleteAsync($"/api/books/{book.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturn401_WhenNotAuthenticated()
        {
            // Act
            var response = await Client.DeleteAsync("/api/books/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturn403_WhenMember()
        {
            // Arrange
            AuthenticateAsMember();

            // Act
            var response = await Client.DeleteAsync("/api/books/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
