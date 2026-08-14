namespace Library.Integration.Tests.Borrowings
{
    public class ReturnBookTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task ReturnBook_ShouldReturnNoContent_WhenValid()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var user = await SeedUserAsync();

            var borrowResponse = await Client.PostAsJsonAsync("/api/borrowings/borrow", new
            {
                bookId = book.Id,
                userId = user.Id
            });
            borrowResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                var borrowing = await db.Borrowings.FirstAsync(b => b.BookId == book.Id && b.UserId == user.Id);

                // Act
                var response = await Client.PutAsJsonAsync($"/api/borrowings/{borrowing.Id}/return", new { });

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }

            using var verifyScope = factory.Services.CreateScope();
            var verifyDb = verifyScope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var returned = await verifyDb.Borrowings.FirstAsync(b => b.BookId == book.Id && b.UserId == user.Id);
            returned.Status.Should().Be(BorrowingStatus.Returned);
            returned.ReturnDate.Should().NotBeNull();

            var updatedBook = await verifyDb.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            updatedBook!.AvailableCopies.Should().Be(5);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturn404_WhenBorrowingNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();

            // Act
            var response = await Client.PutAsJsonAsync("/api/borrowings/9999/return", new { });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturn400_WhenAlreadyReturned()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var user = await SeedUserAsync();

            await Client.PostAsJsonAsync("/api/borrowings/borrow", new
            {
                bookId = book.Id,
                userId = user.Id
            });

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                var borrowing = await db.Borrowings.FirstAsync(b => b.BookId == book.Id && b.UserId == user.Id);

                var firstReturn = await Client.PutAsJsonAsync($"/api/borrowings/{borrowing.Id}/return", new { });
                firstReturn.StatusCode.Should().Be(HttpStatusCode.NoContent);

                // Act
                var response = await Client.PutAsJsonAsync($"/api/borrowings/{borrowing.Id}/return", new { });

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task ReturnBook_ShouldReturn401_WhenNotAuthenticated()
        {
            // Act
            var response = await Client.PutAsJsonAsync("/api/borrowings/1/return", new { });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturn403_WhenMember()
        {
            // Arrange
            AuthenticateAsMember();

            // Act
            var response = await Client.PutAsJsonAsync("/api/borrowings/1/return", new { });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
