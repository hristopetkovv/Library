namespace Library.Infrastructure.Repositories
{
    public class ReviewRepository(LibraryDbContext context) : Repository<Review>(context), IReviewRepository
    {
    }
}
