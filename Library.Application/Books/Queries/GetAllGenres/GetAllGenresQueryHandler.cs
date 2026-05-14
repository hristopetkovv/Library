namespace Library.Application.Books.Queries.GetAllGenres
{
    public class GetAllGenresQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllGenresQuery, List<GenreDto>>
    {
        public async Task<List<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await unitOfWork.Genres.GetAllAsync(cancellationToken);

            return genres.Adapt<List<GenreDto>>();
        }
    }
}
