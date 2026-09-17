namespace Library.Application.Users.Queries.GetFavoriteBooks
{
    public class GetFavoriteBooksQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<GetFavoriteBooksQuery, List<UserFavoriteBookDto>>
    {
        public async Task<List<UserFavoriteBookDto>> Handle(GetFavoriteBooksQuery query, CancellationToken cancellationToken)
        {
            var favorites = await unitOfWork.Users.GetFavoriteBooksAsync(userContext.UserId, cancellationToken);

            return favorites.OrderByDescending(r => r.CreatedAt).Adapt<List<UserFavoriteBookDto>>();
        }
    }
}
