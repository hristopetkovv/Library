namespace Library.Application.Tests.Books.Queries;

public class GetAllGenresQueryHandlerTests
{
    static GetAllGenresQueryHandlerTests()
    {
        BookMappingConfig.Configure();
    }
    private readonly Mock<IGenreRepository> genreRepo;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly GetAllGenresQueryHandler handler;

    public GetAllGenresQueryHandlerTests()
    {
        genreRepo = new Mock<IGenreRepository>();
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Genres).Returns(genreRepo.Object);
        handler = new GetAllGenresQueryHandler(unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllGenres()
    {
        var genres = new List<Genre>
        {
            Genre.Create("Fiction", "Художествена", Category.Fiction),
            Genre.Create("Non-Fiction", "Нехудожествена", Category.NonFiction),
        };

        genreRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(genres);

        var query = new GetAllGenresQuery();
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].GenreName.Should().Be("Fiction");
        result[1].GenreName.Should().Be("Non-Fiction");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoGenres()
    {
        genreRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync([]);

        var query = new GetAllGenresQuery();
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}
