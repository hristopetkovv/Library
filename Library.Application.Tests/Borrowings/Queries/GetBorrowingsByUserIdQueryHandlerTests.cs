namespace Library.Application.Tests.Borrowings.Queries;

public class GetBorrowingsByUserIdQueryHandlerTests
{
    private readonly Mock<IBorrowingRepository> borrowingRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetBorrowingsByUserIdQueryHandler handler;

    static GetBorrowingsByUserIdQueryHandlerTests()
    {
        BorrowingMappingConfig.Configure();
    }

    public GetBorrowingsByUserIdQueryHandlerTests()
    {
        borrowingRepo = new Mock<IBorrowingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Borrowings).Returns(borrowingRepo.Object);

        handler = new GetBorrowingsByUserIdQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBorrowingsByUserId()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890");
        var borrowings = new List<Borrowing>
        {
            BorrowingFactory.Create(1, 1, 1, book: book),
            BorrowingFactory.Create(2, 1, 1, book: book),
        };

        borrowingRepo.Setup(r => r.GetByUserIdAsync(
                1, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(borrowings);

        var query = new GetBorrowingsByUserIdQuery(1, null);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBorrowings()
    {
        borrowingRepo.Setup(r => r.GetByUserIdAsync(
                1, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var query = new GetBorrowingsByUserIdQuery(1, null);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldFilterByStatus()
    {
        var book = BookFactory.Create(1, "Test Book", 1, 1, "1234567890");
        var borrowings = new List<Borrowing>
        {
            BorrowingFactory.Create(1, 1, 1, book: book, status: BorrowingStatus.Returned,
                returnDate: DateTime.UtcNow.AddDays(-1)),
        };

        borrowingRepo.Setup(r => r.GetByUserIdAsync(
                1, BorrowingStatus.Returned, It.IsAny<CancellationToken>()))
            .ReturnsAsync(borrowings);

        var query = new GetBorrowingsByUserIdQuery(1, BorrowingStatus.Returned);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Status.Should().Be(BorrowingStatus.Returned);
    }
}
