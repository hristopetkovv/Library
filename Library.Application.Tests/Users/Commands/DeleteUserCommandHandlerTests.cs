namespace Library.Application.Tests.Users.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly DeleteUserCommandHandler handler;

    public DeleteUserCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new DeleteUserCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenNoActiveBorrowings()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new DeleteUserCommand(1);

        userRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        userRepo.Verify(r => r.Remove(user), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new DeleteUserCommand(99);

        userRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        userRepo.Verify(r => r.Remove(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenUserHasActiveBorrowings()
    {
        var activeBorrowing = Borrowing.Create(1, 1);
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe",
            borrowings: [activeBorrowing]);
        var command = new DeleteUserCommand(1);

        userRepo.Setup(r => r.GetByIdAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.UserHasActiveBorrowings);
        userRepo.Verify(r => r.Remove(It.IsAny<User>()), Times.Never);
    }
}
