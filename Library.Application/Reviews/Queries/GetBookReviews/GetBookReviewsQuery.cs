namespace Library.Application.Reviews.Queries.GetBookReviews
{
    public record GetBookReviewsQuery(int BookId) : IRequest<List<ReviewDto>>;
}
