using System.Linq.Expressions;

namespace Library.Application.Tests.Borrowings.Queries;

public class GetAllBorrowingsQueryHandlerTests
{
    private readonly Mock<IBorrowingRepository> borrowingRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllBorrowingsQueryHandler handler;

    static GetAllBorrowingsQueryHandlerTests()
    {
        BorrowingMappingConfig.Configure();
    }

    public GetAllBorrowingsQueryHandlerTests()
    {
        borrowingRepo = new Mock<IBorrowingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Borrowings).Returns(borrowingRepo.Object);

        handler = new GetAllBorrowingsQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllBorrowings()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890");
        var user = UserFactory.Create(1, "user@test.com", "John", "Doe");
        var borrowings = new List<Borrowing>
        {
            BorrowingFactory.Create(1, 1, 1, book: book, user: user),
            BorrowingFactory.Create(2, 1, 1, book: book, user: user),
        };

        borrowingRepo.Setup(r => r.GetBorrowingsAsync(
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(borrowings);

        var filter = new SearchBorrowingsFilterDto(null, null, null, null);
        var query = new GetAllBorrowingsQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBorrowings()
    {
        borrowingRepo.Setup(r => r.GetBorrowingsAsync(
                It.IsAny<Expression<Func<Borrowing, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var filter = new SearchBorrowingsFilterDto(null, null, null, null);
        var query = new GetAllBorrowingsQuery(filter);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
