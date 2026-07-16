namespace Library.Application.Borrowings.Dtos
{
	public record BorrowingDetailDto(
		int Id,
        BookBorrowingDto Book,
		string UserEmail,
		DateTime BorrowDate,
		DateTime DueDate,
		DateTime? ReturnDate,
		BorrowingStatus Status
	);
}
