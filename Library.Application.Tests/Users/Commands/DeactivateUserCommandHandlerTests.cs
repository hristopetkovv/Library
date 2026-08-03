namespace Library.Application.Tests.Users.Commands;

public class DeactivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly DeactivateUserCommandHandler handler;

    public DeactivateUserCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new DeactivateUserCommandHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeactivateUser_WhenUserExists()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new DeactivateUserCommand(1);

        userRepo.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        user.Status.Should().Be(UserStatus.Inactive);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new DeactivateUserCommand(99);

        userRepo.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
