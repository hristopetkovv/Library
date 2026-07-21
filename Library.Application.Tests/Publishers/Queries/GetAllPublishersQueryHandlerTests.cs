namespace Library.Application.Tests.Publishers.Queries;

public class GetAllPublishersQueryHandlerTests
{
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllPublishersQueryHandler handler;

    public GetAllPublishersQueryHandlerTests()
    {
        publisherRepo = new Mock<IPublisherRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);
        handler = new GetAllPublishersQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllPublishersOrderedByName_WhenNoFilter()
    {
        var publishers = new List<Publisher>
        {
            PublisherFactory.Create(2, "Zoe Publishing"),
            PublisherFactory.Create(1, "Alpha Press"),
        };

        publisherRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(publishers);

        var query = new GetAllPublishersQuery("");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alpha Press");
        result[1].Name.Should().Be("Zoe Publishing");
        result[0].BooksCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldDelegateFilterToRepository()
    {
        var publishers = new List<Publisher>
        {
            PublisherFactory.Create(1, "Penguin Books"),
        };

        publisherRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(publishers);

        var query = new GetAllPublishersQuery("Penguin");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().ContainSingle(p => p.Name == "Penguin Books");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPublishersMatch()
    {
        publisherRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync([]);

        var query = new GetAllPublishersQuery("NonExistent");
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
