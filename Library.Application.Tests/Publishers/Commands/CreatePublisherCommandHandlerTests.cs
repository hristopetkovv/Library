namespace Library.Application.Tests.Publishers.Commands;

public class CreatePublisherCommandHandlerTests
{
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly CreatePublisherCommandHandler handler;

    public CreatePublisherCommandHandlerTests()
    {
        publisherRepo = new Mock<IPublisherRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);
        handler = new CreatePublisherCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreatePublisher_WhenNameIsUnique()
    {
        var command = new CreatePublisherCommand("Test Publisher");

        publisherRepo.Setup(r => r.AnyAsync(p => p.Name == command.Name, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        publisherRepo.Verify(r => r.AddAsync(
            It.Is<Publisher>(p => p.Name == command.Name), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenNameAlreadyExists()
    {
        var command = new CreatePublisherCommand("Duplicate Publisher");

        publisherRepo.Setup(r => r.AnyAsync(p => p.Name == command.Name, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.PublisherWithThatNameExists);
        publisherRepo.Verify(r => r.AddAsync(It.IsAny<Publisher>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
