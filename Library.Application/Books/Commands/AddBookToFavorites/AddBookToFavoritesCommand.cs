namespace Library.Application.Books.Commands.AddBookToFavorites
{
    public record AddBookToFavoritesCommand(int BookId) : IRequest<Unit>;
}
