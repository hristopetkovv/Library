namespace Library.Application.Books.Dtos
{
    public record BookBorrowingDto(
        string Title,
        string Author,
        string Publisher,
        string ISBN
    );
}
