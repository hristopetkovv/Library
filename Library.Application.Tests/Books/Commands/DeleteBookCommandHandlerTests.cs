using System.Linq.Expressions;

namespace Library.Application.Tests.Books.Commands;

public class DeleteBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly DeleteBookCommandHandler handler;

    public DeleteBookCommandHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        handler = new DeleteBookCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteBook_WhenNoActiveBorrowings()
    {
        var book = BookFactory.Create(1, "Book", 1, 1, "1234567890");
        var command = new DeleteBookCommand(1);

        bookRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        bookRepo.Verify(r => r.Remove(book), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenBookDoesNotExist()
    {
        var command = new DeleteBookCommand(99);

        bookRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync((Book?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.BookNotFound);
        bookRepo.Verify(r => r.Remove(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenBookHasActiveBorrowings()
    {
        var activeBorrowing = Borrowing.Create(1, 42);
        var book = BookFactory.Create(1, "Book", 1, 1, "1234567890",
            borrowings: [activeBorrowing]);
        var command = new DeleteBookCommand(1);

        bookRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.BookHasActiveBorrowings);
        bookRepo.Verify(r => r.Remove(It.IsAny<Book>()), Times.Never);
    }
}
