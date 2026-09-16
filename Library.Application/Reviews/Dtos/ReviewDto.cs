namespace Library.Application.Reviews.Dtos
{
    public record ReviewDto(
        int Id,
        int UserId,
        string UserFullName,
        string Content,
        int Rating,
        DateTime CreatedAt
    );
}
