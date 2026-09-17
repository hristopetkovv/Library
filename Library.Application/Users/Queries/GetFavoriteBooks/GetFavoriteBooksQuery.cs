namespace Library.Application.Users.Queries.GetFavoriteBooks
{
    public record GetFavoriteBooksQuery() : IRequest<List<UserFavoriteBookDto>>;
}
