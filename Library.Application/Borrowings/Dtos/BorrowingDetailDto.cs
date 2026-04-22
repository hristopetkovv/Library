namespace Library.Application.Borrowings.Dtos
{
	public record BorrowingDetailDto(
		int Id,
		BookListDto Book,
		string UserEmail,
		DateTime BorrowDate,
		DateTime DueDate,
		DateTime? ReturnDate,
		BorrowingStatus Status
	);
}
