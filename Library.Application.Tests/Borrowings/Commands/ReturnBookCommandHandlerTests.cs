namespace Library.Application.Tests.Borrowings.Commands;

public class ReturnBookCommandHandlerTests
{
    private readonly Mock<IBorrowingRepository> borrowingRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly ReturnBookCommandHandler handler;

    public ReturnBookCommandHandlerTests()
    {
        borrowingRepo = new Mock<IBorrowingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Borrowings).Returns(borrowingRepo.Object);
        unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        handler = new ReturnBookCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBook_WhenValid()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 3);
        var borrowing = BorrowingFactory.Create(1, 1, 1, book: book);

        borrowingRepo.Setup(r => r.GetByIdForUpdateAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Borrowing, object>>[]>()))
            .ReturnsAsync(borrowing);

        var command = new ReturnBookCommand(1);
        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        borrowing.Status.Should().Be(BorrowingStatus.Returned);
        borrowing.ReturnDate.Should().NotBeNull();
        book.AvailableCopies.Should().Be(4);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenBorrowingDoesNotExist()
    {
        borrowingRepo.Setup(r => r.GetByIdForUpdateAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Borrowing, object>>[]>()))
            .ReturnsAsync((Borrowing?)null);

        var command = new ReturnBookCommand(99);
        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.BorrowingNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldRollbackTransaction_WhenExceptionOccurs()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 3);
        var borrowing = BorrowingFactory.Create(1, 1, 1, book: book);

        borrowingRepo.Setup(r => r.GetByIdForUpdateAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Borrowing, object>>[]>()))
            .ReturnsAsync(borrowing);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var command = new ReturnBookCommand(1);
        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
