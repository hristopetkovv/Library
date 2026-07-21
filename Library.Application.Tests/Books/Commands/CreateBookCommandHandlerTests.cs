namespace Library.Application.Tests.Books.Commands;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<ICoverService> coverService;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly CreateBookCommandHandler handler;

    public CreateBookCommandHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        authorRepo = new Mock<IAuthorRepository>();
        publisherRepo = new Mock<IPublisherRepository>();
        coverService = new Mock<ICoverService>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);

        handler = new CreateBookCommandHandler(unitOfWork.Object, coverService.Object);
    }

    private static CreateBookCommand ValidCommand => new(
        Title: "Test Book",
        AuthorId: 1,
        PublisherId: 1,
        ISBN: "1234567890",
        Description: "A test book",
        Pages: 200,
        Language: Language.English,
        CoverType: CoverType.Softcover,
        PublicationYear: 2023,
        TotalCopies: 5,
        AvailableCopies: 5,
        GenreIds: [1, 2]
    );

    [Fact]
    public async Task Handle_ShouldCreateBook_WhenAuthorAndPublisherExist()
    {
        var command = ValidCommand;

        authorRepo.Setup(r => r.GetByIdAsync(command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(Author.Create("Author", "Bio"));
        publisherRepo.Setup(r => r.GetByIdAsync(command.PublisherId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Publisher.Create("Publisher"));
        coverService.Setup(s => s.TryDownloadCoverAsync(command.ISBN))
                    .ReturnsAsync("http://cover.url");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        bookRepo.Verify(r => r.AddAsync(
            It.Is<Book>(b => b.Title == command.Title), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateBook_WhenCoverDownloadReturnsNull()
    {
        var command = ValidCommand;

        authorRepo.Setup(r => r.GetByIdAsync(command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(Author.Create("Author", "Bio"));
        publisherRepo.Setup(r => r.GetByIdAsync(command.PublisherId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Publisher.Create("Publisher"));
        coverService.Setup(s => s.TryDownloadCoverAsync(command.ISBN))
                    .ReturnsAsync((string?)null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        bookRepo.Verify(r => r.AddAsync(
            It.Is<Book>(b => b.CoverImageUrl == null), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenAuthorDoesNotExist()
    {
        var command = ValidCommand;

        authorRepo.Setup(r => r.GetByIdAsync(command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync((Author?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.AuthorNotFound);
        bookRepo.Verify(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenPublisherDoesNotExist()
    {
        var command = ValidCommand;

        authorRepo.Setup(r => r.GetByIdAsync(command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(Author.Create("Author", "Bio"));
        publisherRepo.Setup(r => r.GetByIdAsync(command.PublisherId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Publisher?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.PublisherNotFound);
        bookRepo.Verify(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
