namespace Library.Application.Tests.Books.Queries;

public class GetBookByIdQueryHandlerTests
{
    static GetBookByIdQueryHandlerTests()
    {
        BookMappingConfig.Configure();
    }
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserContext> userContext;
    private readonly GetBookByIdQueryHandler handler;

    private const int TestUserId = 42;

    public GetBookByIdQueryHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        userRepo = new Mock<IUserRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        userContext = new Mock<IUserContext>();

        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        userContext.Setup(c => c.UserId).Returns(TestUserId);

        handler = new GetBookByIdQueryHandler(unitOfWork.Object, userContext.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ShouldReturnBookDetailDto_WhenBookExists(bool expectedIsFavorite)
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890");
        var query = new GetBookByIdQuery(1);

        bookRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);

        userRepo.Setup(r => r.IsFavoriteAsync(TestUserId, query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedIsFavorite);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Book");
        result.IsFavorite.Should().Be(expectedIsFavorite);
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
