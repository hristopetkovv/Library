namespace Library.Application.Tests.Publishers.Commands;

public class UpdatePublisherCommandHandlerTests
{
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly UpdatePublisherCommandHandler handler;

    public UpdatePublisherCommandHandlerTests()
    {
        publisherRepo = new Mock<IPublisherRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);
        handler = new UpdatePublisherCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePublisher_WhenValid()
    {
        var existingPublisher = PublisherFactory.Create(1, "Old Name");
        var command = new UpdatePublisherCommand(1, "Updated Name");

        publisherRepo.Setup(r => r.GetByIdForUpdateAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(existingPublisher);
        publisherRepo.Setup(r => r.FirstOrDefaultAsync(
            p => p.Name == command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Publisher?)null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        existingPublisher.Name.Should().Be("Updated Name");
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePublisher_WhenNameBelongsToSamePublisher()
    {
        var existingPublisher = PublisherFactory.Create(1, "Same Name");
        var command = new UpdatePublisherCommand(1, "Same Name");

        publisherRepo.Setup(r => r.GetByIdForUpdateAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(existingPublisher);
        publisherRepo.Setup(r => r.FirstOrDefaultAsync(
            p => p.Name == command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPublisher);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenPublisherDoesNotExist()
    {
        var command = new UpdatePublisherCommand(99, "Name");

        publisherRepo.Setup(r => r.GetByIdForUpdateAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync((Publisher?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.PublisherNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenAnotherPublisherWithSameNameExists()
    {
        var existingPublisher = PublisherFactory.Create(1, "Original");
        var otherPublisher = PublisherFactory.Create(2, "Conflicting Name");
        var command = new UpdatePublisherCommand(1, "Conflicting Name");

        publisherRepo.Setup(r => r.GetByIdForUpdateAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(existingPublisher);
        publisherRepo.Setup(r => r.FirstOrDefaultAsync(
            p => p.Name == command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(otherPublisher);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.PublisherWithThatNameExists);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
