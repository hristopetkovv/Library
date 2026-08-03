using System.Linq.Expressions;

namespace Library.Application.Tests.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserContext> userContext;
    private readonly UpdateUserCommandHandler handler;

    static UpdateUserCommandHandlerTests()
    {
        UserMappingConfig.Configure();
    }

    public UpdateUserCommandHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        userContext = new Mock<IUserContext>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new UpdateUserCommandHandler(unitOfWork.Object, userContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValid()
    {
        var user = UserFactory.Create(1, "old@test.com", "Old", "Name");
        var command = new UpdateUserCommand("new@test.com", "New", "Name", "New Address", "9876543210");

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
        userRepo.Setup(r => r.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Email.Should().Be("new@test.com");
        result.FirstName.Should().Be("New");
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var command = new UpdateUserCommand("test@test.com", "Name", "Name", null, null);

        userContext.Setup(x => x.UserId).Returns(99);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequest_WhenEmailAlreadyExists()
    {
        var user = UserFactory.Create(1, "old@test.com", "Old", "Name");
        var command = new UpdateUserCommand("existing@test.com", "New", "Name", null, null);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
        userRepo.Setup(r => r.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
                 .WithMessage(ValidationMessages.UserEmailExists);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
