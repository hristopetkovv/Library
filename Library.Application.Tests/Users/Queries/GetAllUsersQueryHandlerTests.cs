namespace Library.Application.Tests.Users.Queries;

public class GetAllUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> userRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllUsersQueryHandler handler;

    public GetAllUsersQueryHandlerTests()
    {
        userRepo = new Mock<IUserRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Users).Returns(userRepo.Object);
        handler = new GetAllUsersQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllUsers_WhenFilterProvided()
    {
        var users = new List<User>
        {
            UserFactory.Create(1, "alice@test.com", "Alice", "Smith"),
            UserFactory.Create(2, "bob@test.com", "Bob", "Jones"),
        };

        userRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var filter = new SearchUsersFilterDto(null, null);
        var query = new GetAllUsersQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersMatch()
    {
        userRepo.Setup(r => r.GetAllFilteredAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var filter = new SearchUsersFilterDto("nonexistent@test.com", null);
        var query = new GetAllUsersQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
