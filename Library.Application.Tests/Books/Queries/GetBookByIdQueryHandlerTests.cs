namespace Library.Application.Tests.Books.Queries;

public class GetBookByIdQueryHandlerTests
{
    static GetBookByIdQueryHandlerTests()
    {
        BookMappingConfig.Configure();
    }
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetBookByIdQueryHandler handler;

    public GetBookByIdQueryHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        handler = new GetBookByIdQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBookDetailDto_WhenBookExists()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890");
        var query = new GetBookByIdQuery(1);

        bookRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Book");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        var query = new GetBookByIdQuery(99);

        bookRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync((Book?)null);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"*{nameof(Book)}*")
                 .WithMessage($"*99*");
    }
}
