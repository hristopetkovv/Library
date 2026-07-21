using System.Reflection;

namespace Library.Application.Tests.Publishers;

public static class PublisherFactory
{
    public static Publisher Create(int id, string name, List<Book>? books = null)
    {
        var publisher = Publisher.Create(name);

        typeof(Publisher)
            .GetProperty(nameof(Publisher.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(publisher, id);

        if (books is not null)
        {
            var field = typeof(Publisher).GetField("books", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var list = (List<Book>)field.GetValue(publisher)!;
                list.AddRange(books);
            }
        }

        return publisher;
    }
}
