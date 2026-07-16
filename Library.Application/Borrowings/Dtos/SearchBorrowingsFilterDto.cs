namespace Library.Application.Borrowings.Dtos
{
    public record SearchBorrowingsFilterDto(
        string? BookTitle,
        string? AuthorName,
        string? UserEmail,
        string? ISBN
    )
    {
        public Expression<Func<Borrowing, bool>> Predicate()
        {
            return b =>
            (string.IsNullOrEmpty(BookTitle) || b.Book.Title.ToLower().Contains(BookTitle.ToLower()))
            && (string.IsNullOrEmpty(AuthorName) || b.Book.Author.Name.ToLower().Contains(AuthorName.ToLower()))
            && (string.IsNullOrEmpty(ISBN) || b.Book.ISBN.Value.ToLower().Contains(ISBN.ToLower()))
            && (string.IsNullOrEmpty(UserEmail) || b.User.Email.Value.ToLower().Contains(UserEmail.ToLower()));
        }
    }
}
