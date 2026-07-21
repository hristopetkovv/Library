namespace Library.Application.Tests.Authors.Queries;

public class GetAuthorByIdQueryHandlerTests
{
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAuthorByIdQueryHandler handler;

    public GetAuthorByIdQueryHandlerTests()
    {
        authorRepo = new Mock<IAuthorRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        handler = new GetAuthorByIdQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthorDetailDto_WhenAuthorExists()
    {
        var author = AuthorFactory.Create(1, "Test Author", "Test Biography");
        var query = new GetAuthorByIdQuery(1);

        authorRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Author, object>>[]>()))
            .ReturnsAsync(author);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Author");
        result.Biography.Should().Be("Test Biography");
        result.Books.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
    {
        var query = new GetAuthorByIdQuery(99);

        authorRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Author, object>>[]>()))
            .ReturnsAsync((Author?)null);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"*{nameof(Author)}*")
                 .WithMessage($"*99*");
    }
}
