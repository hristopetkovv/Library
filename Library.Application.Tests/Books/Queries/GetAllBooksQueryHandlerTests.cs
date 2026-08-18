namespace Library.Application.Tests.Books.Queries;

public class GetAllBooksQueryHandlerTests
{
    static GetAllBooksQueryHandlerTests()
    {
        BookMappingConfig.Configure();
    }
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllBooksQueryHandler handler;

    public GetAllBooksQueryHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        handler = new GetAllBooksQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllBooks_WhenFilterProvided()
    {
        var books = new List<Book>
        {
            BookFactory.Create(1, "Book One", 1, 1, "1234567890"),
            BookFactory.Create(2, "Book Two", 1, 1, "0987654321"),
        };

        bookRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(books);

        var filter = new SearchBooksFilterDto(null, null, null, null, null);
        var query = new GetAllBooksQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Book One");
        result[1].Title.Should().Be("Book Two");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBooksMatch()
    {
        bookRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync([]);

        var filter = new SearchBooksFilterDto("NonExistent", null, null, null, null);
        var query = new GetAllBooksQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
