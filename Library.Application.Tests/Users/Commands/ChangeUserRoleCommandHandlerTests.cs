namespace Library.Application.Tests.Users.Commands;

public class ChangeUserRoleCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserContext> userContext;
    private readonly ChangeUserRoleCommandHandler handler;

    public ChangeUserRoleCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        userContext = new Mock<IUserContext>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new ChangeUserRoleCommandHandler(unitOfWork.Object, userContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldChangeRole_WhenValid()
    {
        var user = UserFactory.Create(2, "test@test.com", "John", "Doe", role: UserRole.Member);
        var command = new ChangeUserRoleCommand(2, UserRole.Admin);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        user.Role.Should().Be(UserRole.Admin);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidden_WhenChangingOwnRole()
    {
        var command = new ChangeUserRoleCommand(1, UserRole.Admin);

        userContext.Setup(x => x.UserId).Returns(1);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>()
                 .WithMessage(ValidationMessages.UserCannotChangeOwnRole);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new ChangeUserRoleCommand(99, UserRole.Admin);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
