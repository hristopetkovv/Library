namespace Library.Application.Tests.Publishers.Queries;

public class GetPublisherByIdQueryHandlerTests
{
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetPublisherByIdQueryHandler handler;

    public GetPublisherByIdQueryHandlerTests()
    {
        publisherRepo = new Mock<IPublisherRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);
        handler = new GetPublisherByIdQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPublisherDetailDto_WhenPublisherExists()
    {
        var publisher = PublisherFactory.Create(1, "Test Publisher");
        var query = new GetPublisherByIdQuery(1);

        publisherRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(publisher);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Publisher");
        result.Books.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPublisherDoesNotExist()
    {
        var query = new GetPublisherByIdQuery(99);

        publisherRepo.Setup(r => r.GetByIdAsync(
                query.Id,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync((Publisher?)null);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"*{nameof(Publisher)}*")
                 .WithMessage($"*99*");
    }
}
