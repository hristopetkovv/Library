namespace Library.Application.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(int ReviewId) : IRequest<Unit>;
}
