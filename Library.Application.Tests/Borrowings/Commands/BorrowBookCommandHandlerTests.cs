using System.Linq.Expressions;

namespace Library.Application.Tests.Borrowings.Commands;

public class BorrowBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IBorrowingRepository> borrowingRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly BorrowBookCommandHandler handler;

    public BorrowBookCommandHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        userRepo = new Mock<IUserRepository>();
        borrowingRepo = new Mock<IBorrowingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        unitOfWork.Setup(u => u.Borrowings).Returns(borrowingRepo.Object);
        unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        handler = new BorrowBookCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldBorrowBook_WhenValid()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 5);
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new BorrowBookCommand(1, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        userRepo.Setup(r => r.GetByIdAsync(
                command.UserId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        borrowingRepo.Verify(r => r.AddAsync(
            It.Is<Borrowing>(b => b.BookId == command.BookId && b.UserId == command.UserId),
            It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        book.AvailableCopies.Should().Be(4);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenBookDoesNotExist()
    {
        var command = new BorrowBookCommand(99, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"Book with key '{command.BookId}' was not found.");
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenNoAvailableCopies()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 0);
        var command = new BorrowBookCommand(1, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.BookHasNoAvailableCopies);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 5);
        var command = new BorrowBookCommand(1, 99);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        userRepo.Setup(r => r.GetByIdAsync(
                command.UserId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenUserCannotBorrowMore()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 5);
        var activeBorrowings = Enumerable.Range(1, 5).Select(i =>
            BorrowingFactory.Create(i, 1, 1)).ToList();
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe",
            borrowings: activeBorrowings);
        var command = new BorrowBookCommand(1, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        userRepo.Setup(r => r.GetByIdAsync(
                command.UserId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.UserCannotBorrowMore);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenUserHasOverdueBooks()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 5);
        var overdueBorrowing = BorrowingFactory.Create(1, 1, 1,
            dueDate: DateTime.UtcNow.AddDays(-10));
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe",
            borrowings: [overdueBorrowing]);
        var command = new BorrowBookCommand(1, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        userRepo.Setup(r => r.GetByIdAsync(
                command.UserId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.UserHasOverdueBooks);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldRollbackTransaction_WhenExceptionOccurs()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890",
            totalCopies: 5, availableCopies: 5);
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new BorrowBookCommand(1, 1);

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.BookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        userRepo.Setup(r => r.GetByIdAsync(
                command.UserId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);
        borrowingRepo.Setup(r => r.AddAsync(
                It.IsAny<Borrowing>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
