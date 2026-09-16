namespace Library.Application.Reviews.Queries.GetBookReviews
{
    public class GetBookReviewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookReviewsQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetBookReviewsQuery query, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Reviews.GetAllFilteredAsync(r => r.BookId == query.BookId, cancellationToken, r => r.User);

            return reviews.Adapt<List<ReviewDto>>();
        }
    }
}
