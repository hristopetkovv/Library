namespace Library.Application.Tests.Borrowings;

public static class BorrowingFactory
{
    public static Borrowing Create(int id, int bookId, int userId,
        BorrowingStatus status = BorrowingStatus.Borrowed,
        DateTime? dueDate = null,
        Book? book = null,
        User? user = null,
        DateTime? returnDate = null)
    {
        var borrowing = Borrowing.Create(bookId, userId);

        SetProperty(borrowing, nameof(Borrowing.Id), id);

        if (status != BorrowingStatus.Borrowed)
            SetProperty(borrowing, nameof(Borrowing.Status), status);

        if (dueDate.HasValue)
            SetProperty(borrowing, nameof(Borrowing.DueDate), dueDate.Value);

        if (returnDate.HasValue)
            SetProperty(borrowing, nameof(Borrowing.ReturnDate), returnDate.Value);

        if (book is not null)
            SetProperty(borrowing, nameof(Borrowing.Book), book);

        if (user is not null)
            SetProperty(borrowing, nameof(Borrowing.User), user);

        return borrowing;
    }

    private static void SetProperty<T>(T obj, string propertyName, object value)
    {
        typeof(T)
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(obj, value);
    }
}
