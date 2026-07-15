namespace Library.Application.Borrowings.Dtos
{
    public record BorrowingBasicDto(
        string BookTitle,
        DateTime BorrowDate,
        DateTime DueDate,
        DateTime? ReturnDate,
        BorrowingStatus Status
    );
}
