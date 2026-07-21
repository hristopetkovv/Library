namespace Library.Application.Tests.Books;

public static class BookFactory
{
    public static Book Create(int id, string title, int authorId, int publisherId,
        string isbn, int totalCopies = 5, int availableCopies = 5,
        List<int>? genreIds = null, List<Borrowing>? borrowings = null)
    {
        var book = Book.Create(
            title,
            authorId,
            publisherId,
            ISBN.Create(isbn),
            null,
            200,
            Language.English,
            CoverType.Softcover,
            2023,
            totalCopies,
            null,
            genreIds ?? []);

        typeof(Book)
            .GetProperty(nameof(Book.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(book, id);

        if (availableCopies != totalCopies)
        {
            typeof(Book)
                .GetProperty(nameof(Book.AvailableCopies), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                ?.SetValue(book, availableCopies);
        }

        if (borrowings is not null)
        {
            var field = typeof(Book).GetField("borrowings", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var list = (List<Borrowing>)field.GetValue(book)!;
                list.AddRange(borrowings);
            }
        }

        return book;
    }
}
