namespace Library.Application.Tests.Authors;

public static class AuthorFactory
{
    public static Author Create(int id, string name, string biography, List<Book>? books = null)
    {
        var author = Author.Create(name, biography);

        typeof(Author)
            .GetProperty(nameof(Author.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(author, id);

        if (books is not null)
        {
            var field = typeof(Author).GetField("books", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var list = (List<Book>)field.GetValue(author)!;
                list.AddRange(books);
            }
        }

        return author;
    }
}
