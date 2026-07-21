namespace Library.Application.Tests.Authors.Commands;

public class UpdateAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly UpdateAuthorCommandHandler handler;

    public UpdateAuthorCommandHandlerTests()
    {
        authorRepo = new Mock<IAuthorRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        handler = new UpdateAuthorCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAuthor_WhenValid()
    {
        var existingAuthor = AuthorFactory.Create(1, "Old Name", "Old Biography");
        var command = new UpdateAuthorCommand(1, "Updated Name", "Updated Biography");

        authorRepo.Setup(r => r.GetByIdForUpdateAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync(existingAuthor);
        authorRepo.Setup(r => r.FirstOrDefaultAsync(a => a.Name == command.Name, It.IsAny<CancellationToken>()))
                  .ReturnsAsync((Author?)null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        existingAuthor.Name.Should().Be("Updated Name");
        existingAuthor.Biography.Should().Be("Updated Biography");
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAuthor_WhenNameBelongsToSameAuthor()
    {
        var existingAuthor = AuthorFactory.Create(1, "Same Name", "Biography");
        var command = new UpdateAuthorCommand(1, "Same Name", "Updated Biography");

        authorRepo.Setup(r => r.GetByIdForUpdateAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync(existingAuthor);
        authorRepo.Setup(r => r.FirstOrDefaultAsync(a => a.Name == command.Name, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(existingAuthor);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenAuthorDoesNotExist()
    {
        var command = new UpdateAuthorCommand(99, "Name", "Biography");

        authorRepo.Setup(r => r.GetByIdForUpdateAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync((Author?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.AuthorNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenAnotherAuthorWithSameNameExists()
    {
        var existingAuthor = AuthorFactory.Create(1, "Original", "Bio");
        var otherAuthor = AuthorFactory.Create(2, "Conflicting Name", "Other Bio");
        var command = new UpdateAuthorCommand(1, "Conflicting Name", "Updated Bio");

        authorRepo.Setup(r => r.GetByIdForUpdateAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync(existingAuthor);
        authorRepo.Setup(r => r.FirstOrDefaultAsync(a => a.Name == command.Name, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(otherAuthor);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.AuthorWithThatNameExists);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
