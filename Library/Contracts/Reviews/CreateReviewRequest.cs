namespace Library.Contracts.Reviews
{
    public record CreateReviewRequest(int BookId, string Content, int Rating);
}
