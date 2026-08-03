namespace Library.Application.Tests.Users.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserContext> userContext;
    private readonly GetUserByIdQueryHandler handler;

    static GetUserByIdQueryHandlerTests()
    {
        UserMappingConfig.Configure();
    }

    public GetUserByIdQueryHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        userContext = new Mock<IUserContext>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new GetUserByIdQueryHandler(unitOfWork.Object, userContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDetailDto_WhenOwnProfile()
    {
        var user = UserFactory.Create(1, "test@test.com", "John", "Doe");
        var query = new GetUserByIdQuery(1);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDetailDto_WhenTargetUserIsAnAdmin()
    {
        var adminUser = UserFactory.Create(2, "admin@test.com", "Admin", "User", role: UserRole.Admin);
        var query = new GetUserByIdQuery(2);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(adminUser);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(2);
        result.Email.Should().Be("admin@test.com");
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidden_WhenNonAdminRequestsOtherMemberProfile()
    {
        var memberUser = UserFactory.Create(2, "other@test.com", "Other", "User", role: UserRole.Member);
        var query = new GetUserByIdQuery(2);

        userContext.Setup(x => x.UserId).Returns(1);
        userContext.Setup(x => x.Role).Returns(UserRole.Member);
        userRepo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(memberUser);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>()
                 .WithMessage(ValidationMessages.UserViewOwnProfileOnly);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        var query = new GetUserByIdQuery(99);

        userContext.Setup(x => x.UserId).Returns(1);
        userRepo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.UserNotFound);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidden_WhenNonAdminRequestsOtherProfile()
    {
        var user = UserFactory.Create(2, "other@test.com", "Other", "User", role: UserRole.Member);
        var query = new GetUserByIdQuery(2);

        userContext.Setup(x => x.UserId).Returns(1);
        userContext.Setup(x => x.Role).Returns(UserRole.Member);
        userRepo.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        var act = () => handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>()
                 .WithMessage(ValidationMessages.UserViewOwnProfileOnly);
    }
}
