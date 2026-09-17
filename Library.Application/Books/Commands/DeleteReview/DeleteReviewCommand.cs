namespace Library.Application.Books.Commands.DeleteReview
{
    public record DeleteReviewCommand(int BookId, int ReviewId) : IRequest<Unit>;
}
