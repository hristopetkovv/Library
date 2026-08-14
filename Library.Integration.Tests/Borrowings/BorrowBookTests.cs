namespace Library.Integration.Tests.Borrowings
{
    public class BorrowBookTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task BorrowBook_ShouldReturnNoContent_WhenValid()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var user = await SeedUserAsync();

            var request = new
            {
                bookId = book.Id,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var borrowing = await db.Borrowings.FirstOrDefaultAsync(b => b.BookId == book.Id && b.UserId == user.Id);
            borrowing.Should().NotBeNull();
            borrowing!.Status.Should().Be(BorrowingStatus.Borrowed);

            var updatedBook = await db.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            updatedBook!.AvailableCopies.Should().Be(4);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn404_WhenBookNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var user = await SeedUserAsync();

            var request = new
            {
                bookId = 9999,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn404_WhenUserNotFound()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);

            var request = new
            {
                bookId = book.Id,
                userId = 9999
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn400_WhenBookHasNoAvailableCopies()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id], totalCopies: 0);
            var user = await SeedUserAsync();

            var request = new
            {
                bookId = book.Id,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn400_WhenUserCannotBorrowMore()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var bookWithBorrowings = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var bookToBorrow = await SeedBookAsync(author.Id, publisher.Id, [genre.Id], isbn: "9780261102218");
            var user = await SeedUserAsync();

            for (var i = 0; i < 5; i++)
            {
                await SeedBorrowingAsync(bookWithBorrowings.Id, user.Id);
            }

            var request = new
            {
                bookId = bookToBorrow.Id,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn400_WhenUserHasOverdueBooks()
        {
            // Arrange
            AuthenticateAsAdmin();
            var author = await SeedAuthorAsync();
            var publisher = await SeedPublisherAsync();
            var genre = await SeedGenreAsync();
            var book = await SeedBookAsync(author.Id, publisher.Id, [genre.Id]);
            var user = await SeedUserAsync();
            await SeedBorrowingAsync(book.Id, user.Id, borrowPeriodDays: -30);

            var request = new
            {
                bookId = book.Id,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn400_WhenValidationFails()
        {
            // Arrange
            AuthenticateAsAdmin();
            var user = await SeedUserAsync();

            var request = new
            {
                bookId = 0,
                userId = user.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn401_WhenNotAuthenticated()
        {
            // Arrange
            var request = new { bookId = 1, userId = 1 };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturn403_WhenMember()
        {
            // Arrange
            AuthenticateAsMember();
            var request = new { bookId = 1, userId = 1 };

            // Act
            var response = await Client.PostAsJsonAsync("/api/borrowings/borrow", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
