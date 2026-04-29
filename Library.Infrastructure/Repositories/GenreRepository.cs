namespace Library.Infrastructure.Repositories
{
	public class GenreRepository(LibraryDbContext context) : Repository<Genre>(context), IGenreRepository
	{
	}
}
