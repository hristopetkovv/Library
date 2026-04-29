namespace Library.Domain.Entities.Books
{
	public class Genre : BaseAuditableEntity, IEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; } = null!;

		public string NameBg { get; private set; } = null!;

		public Category Category { get; private set; }

		public ICollection<Book> Books { get; private set; } = [];

		public static Genre Create(string name, string nameBg, Category category)
		{
			return new Genre
			{
				Name = name,
				NameBg = nameBg,
				Category = category
			};
		}
	}
}
