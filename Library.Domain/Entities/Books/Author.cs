namespace Library.Domain.Entities.Books
{
	public class Author : BaseAuditableEntity, IEntity
	{
		private readonly List<Book> books = [];

		public int Id { get; private set; }
		public string Name { get; private set; } = null!;
		public string Biography { get; private set; } = null!;
		public IReadOnlyList<Book> Books => books.AsReadOnly();

		public static Author Create(string name, string biography)
		{
			return new Author
			{
				Name = name,
				Biography = biography
			};
		}

		public void Update(string name, string biography)
		{
			Name = name;
			Biography = biography;
		}
	}
}
