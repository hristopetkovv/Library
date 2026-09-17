namespace Library.Application.Books.Queries.GetBookReviews
{
    public record GetBookReviewsQuery(int BookId) : IRequest<List<ReviewDto>>;
}
