namespace Library.Application.Users.Dtos
{
    public record UserFavoriteBookDto(
        string Title,
        string Author,
        int PublicationYear,
        DateTime CreatedAt
    );
}
