namespace Library.Application.Books.Queries.GetAllGenres
{
    public record GetAllGenresQuery() : IRequest<List<GenreDto>>;
}
