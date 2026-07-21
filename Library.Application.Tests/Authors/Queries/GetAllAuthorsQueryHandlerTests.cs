namespace Library.Application.Tests.Authors.Queries;

public class GetAllAuthorsQueryHandlerTests
{
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllAuthorsQueryHandler handler;

    public GetAllAuthorsQueryHandlerTests()
    {
        authorRepo = new Mock<IAuthorRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        handler = new GetAllAuthorsQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllAuthorsOrderedByName_WhenNoFilter()
    {
        var authors = new List<Author>
        {
            AuthorFactory.Create(2, "Zoe Author", "Bio 2"),
            AuthorFactory.Create(1, "Alice Author", "Bio 1"),
        };

        authorRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Author, object>>[]>()))
            .ReturnsAsync(authors);

        var query = new GetAllAuthorsQuery("");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alice Author");
        result[1].Name.Should().Be("Zoe Author");
        result[0].BooksCount.Should().Be(0);
        result[1].BooksCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldDelegateFilterToRepository()
    {
        var authors = new List<Author>
        {
            AuthorFactory.Create(1, "George Orwell", "Bio"),
        };

        authorRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Author, object>>[]>()))
            .ReturnsAsync(authors);

        var query = new GetAllAuthorsQuery("Orwell");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().ContainSingle(a => a.Name == "George Orwell");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAuthorsMatch()
    {
        authorRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Author, object>>[]>()))
            .ReturnsAsync([]);

        var query = new GetAllAuthorsQuery("NonExistent");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
