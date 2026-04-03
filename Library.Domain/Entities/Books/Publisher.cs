namespace Library.Domain.Entities.Books
{
	public class Publisher : BaseAuditableEntity
	{
		private readonly List<Book> books = [];

		public int Id { get; private set; }
		public string Name { get; private set; } = null!;
		public IReadOnlyList<Book> Books => books.AsReadOnly();

		public static Publisher Create(string name)
		{
			return new Publisher
			{
				Name = name
			};
		}
	}
}
