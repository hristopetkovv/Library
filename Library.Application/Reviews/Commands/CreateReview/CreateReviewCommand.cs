namespace Library.Application.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand(int BookId, string Content, int Rating) : IRequest<Unit>;
}
