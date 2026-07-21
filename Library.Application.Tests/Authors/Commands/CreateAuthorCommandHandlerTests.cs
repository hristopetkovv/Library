namespace Library.Application.Tests.Authors.Commands;

public class CreateAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly CreateAuthorCommandHandler handler;

    public CreateAuthorCommandHandlerTests()
    {
        authorRepo = new Mock<IAuthorRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        handler = new CreateAuthorCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateAuthor_WhenNameIsUnique()
    {
        var command = new CreateAuthorCommand("Test Author", "Test Biography");

        authorRepo.Setup(r => r.AnyAsync(a => a.Name == command.Name, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(false);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        authorRepo.Verify(r => r.AddAsync(It.Is<Author>(a =>
            a.Name == command.Name && a.Biography == command.Biography), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenNameAlreadyExists()
    {
        var command = new CreateAuthorCommand("Duplicate Author", "Biography");

        authorRepo.Setup(r => r.AnyAsync(a => a.Name == command.Name, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.AuthorWithThatNameExists);
        authorRepo.Verify(r => r.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
