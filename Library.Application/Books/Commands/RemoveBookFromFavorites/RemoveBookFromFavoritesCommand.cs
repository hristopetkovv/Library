namespace Library.Application.Books.Commands.RemoveBookFromFavorites
{
    public record RemoveBookFromFavoritesCommand(int BookId) : IRequest<Unit>;
}
