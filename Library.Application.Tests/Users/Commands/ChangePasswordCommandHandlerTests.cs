namespace Library.Application.Tests.Users.Commands;

public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IPasswordHasher> passwordHasher;
    private readonly Mock<IUserContext> userContext;
    private readonly ChangePasswordCommandHandler handler;

    public ChangePasswordCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        passwordHasher = new Mock<IPasswordHasher>();
        userContext = new Mock<IUserContext>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new ChangePasswordCommandHandler(unitOfWork.Object, passwordHasher.Object, userContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldChangePassword_WhenCurrentPasswordIsValid()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new ChangePasswordCommand("CurrentPass1!", "NewPass1!");

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
        passwordHasher.Setup(p => p.VerifyPassword(command.CurrentPassword, It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(true);
        passwordHasher.Setup(p => p.HashPassword(command.NewPassword, out It.Ref<string>.IsAny!))
                      .Returns("newHash");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new ChangePasswordCommand("CurrentPass1!", "NewPass1!");

        userContext.Setup(x => x.UserId).Returns(99);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenCurrentPasswordIsInvalid()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var command = new ChangePasswordCommand("WrongPass1!", "NewPass1!");

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
        passwordHasher.Setup(p => p.VerifyPassword(command.CurrentPassword, It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(false);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.InvalidCurrentPassword);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
