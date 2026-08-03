namespace Library.Application.Tests.Users.Commands;

public class ActivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly ActivateUserCommandHandler handler;

    public ActivateUserCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new ActivateUserCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldActivateUser_WhenUserExists()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe", status: UserStatus.Inactive);
        var command = new ActivateUserCommand(1);

        userRepo.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        user.Status.Should().Be(UserStatus.Active);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new ActivateUserCommand(99);

        userRepo.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
