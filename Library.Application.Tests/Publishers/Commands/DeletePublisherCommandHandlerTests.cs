namespace Library.Application.Tests.Publishers.Commands;

public class DeletePublisherCommandHandlerTests
{
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly DeletePublisherCommandHandler handler;

    public DeletePublisherCommandHandlerTests()
    {
        publisherRepo = new Mock<IPublisherRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);
        handler = new DeletePublisherCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeletePublisher_WhenNoAssociatedBooks()
    {
        var publisher = PublisherFactory.Create(1, "Publisher", []);
        var command = new DeletePublisherCommand(1);

        publisherRepo.Setup(r => r.GetByIdAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(publisher);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        publisherRepo.Verify(r => r.Remove(publisher), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenPublisherDoesNotExist()
    {
        var command = new DeletePublisherCommand(99);

        publisherRepo.Setup(r => r.GetByIdAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync((Publisher?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.PublisherNotFound);
        publisherRepo.Verify(r => r.Remove(It.IsAny<Publisher>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenPublisherHasAssociatedBooks()
    {
        var books = new List<Book> { new Book() };
        var publisher = PublisherFactory.Create(1, "Publisher", books);
        var command = new DeletePublisherCommand(1);

        publisherRepo.Setup(r => r.GetByIdAsync(
            command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Publisher, object>>[]>()))
            .ReturnsAsync(publisher);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.PublisherHasAssociatedBooks);
        publisherRepo.Verify(r => r.Remove(It.IsAny<Publisher>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
