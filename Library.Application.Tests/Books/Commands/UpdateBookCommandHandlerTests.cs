namespace Library.Application.Tests.Books.Commands;

public class UpdateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> bookRepo;
    private readonly Mock<IAuthorRepository> authorRepo;
    private readonly Mock<IPublisherRepository> publisherRepo;
    private readonly Mock<ICoverService> coverService;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly UpdateBookCommandHandler handler;

    public UpdateBookCommandHandlerTests()
    {
        bookRepo = new Mock<IBookRepository>();
        authorRepo = new Mock<IAuthorRepository>();
        publisherRepo = new Mock<IPublisherRepository>();
        coverService = new Mock<ICoverService>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Books).Returns(bookRepo.Object);
        unitOfWork.Setup(u => u.Authors).Returns(authorRepo.Object);
        unitOfWork.Setup(u => u.Publishers).Returns(publisherRepo.Object);

        handler = new UpdateBookCommandHandler(unitOfWork.Object, coverService.Object);
    }

    private static UpdateBookCommand ValidCommand => new(
        Id: 1,
        Title: "Updated Book",
        AuthorId: 1,
        PublisherId: 1,
        ISBN: "1234567890",
        Description: "Updated description",
        Pages: 250,
        Language: Language.English,
        CoverType: CoverType.Hardcover,
        PublicationYear: 2024,
        TotalCopies: 10,
        AvailableCopies: 8,
        GenreIds: [1, 2, 3]
    );

    [Fact]
    public async Task Handle_ShouldUpdateBook_WhenAllPropertiesExist()
    {
        var command = ValidCommand;
        var book = BookFactory.Create(1, "Old Title", 1, 1, "1234567890");

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);
        authorRepo.Setup(r => r.AnyAsync(a => a.Id == command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);
        publisherRepo.Setup(r => r.AnyAsync(p => p.Id == command.PublisherId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);
        coverService.Setup(s => s.TryDownloadCoverAsync(command.ISBN))
                    .ReturnsAsync("http://cover.url");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
        book.Title.Should().Be("Updated Book");
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenBookDoesNotExist()
    {
        var command = ValidCommand;

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync((Book?)null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.BookNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenAuthorDoesNotExist()
    {
        var command = ValidCommand;
        var book = BookFactory.Create(1, "Old Title", 1, 1, "1234567890");

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);
        authorRepo.Setup(r => r.AnyAsync(a => a.Id == command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(false);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.AuthorNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenPublisherDoesNotExist()
    {
        var command = ValidCommand;
        var book = BookFactory.Create(1, "Old Title", 1, 1, "1234567890");

        bookRepo.Setup(r => r.GetByIdForUpdateAsync(
                command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Book, object>>[]>()))
            .ReturnsAsync(book);
        authorRepo.Setup(r => r.AnyAsync(a => a.Id == command.AuthorId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);
        publisherRepo.Setup(r => r.AnyAsync(p => p.Id == command.PublisherId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(ValidationMessages.PublisherNotFound);
        unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
