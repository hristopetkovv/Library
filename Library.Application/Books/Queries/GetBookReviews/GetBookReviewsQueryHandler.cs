namespace Library.Application.Books.Queries.GetBookReviews
{
    public class GetBookReviewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookReviewsQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetBookReviewsQuery query, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Books.GetReviewsAsync(query.BookId, cancellationToken);

            return reviews.OrderByDescending(r => r.CreatedAt).Adapt<List<ReviewDto>>();
        }
    }
}
