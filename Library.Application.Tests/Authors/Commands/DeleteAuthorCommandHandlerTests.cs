namespace Library.Application.Tests.Authors.Commands;

public class DeleteAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly DeleteAuthorCommandHandler handler;

    public DeleteAuthorCommandHandlerTests()
    {
        authorRepo = new Mock<IAuthorRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        handler = new DeleteAuthorCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAuthor_WhenNoAssociatedBooks()
    {
        var author = AuthorFactory.Create(1, "Author", "Bio", []);
        var command = new DeleteAuthorCommand(1);

        authorRepo.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync(author);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        authorRepo.Verify(r => r.Remove(author), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenAuthorDoesNotExist()
    {
        var command = new DeleteAuthorCommand(99);

        authorRepo.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync((Author?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.AuthorNotFound);
        authorRepo.Verify(r => r.Remove(It.IsAny<Author>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenAuthorHasAssociatedBooks()
    {
        var books = new List<Book> { new Book() }; // Book with default Id = 0 is enough to trigger Any()
        var author = AuthorFactory.Create(1, "Author", "Bio", books);
        var command = new DeleteAuthorCommand(1);

        authorRepo.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
                  .ReturnsAsync(author);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.AuthorHasAssociatedBooks);
        authorRepo.Verify(r => r.Remove(It.IsAny<Author>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
