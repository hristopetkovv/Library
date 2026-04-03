namespace Library.Infrastructure.Persistence.Configurations
{
	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.Property(b => b.Title)
				.IsRequired()
				.HasMaxLength(300);

			builder.OwnsOne(b => b.ISBN, isbn =>
			{
				isbn.Property(i => i.Value)
					.HasColumnName("ISBN")
					.IsRequired()
					.HasMaxLength(13);

				isbn.HasIndex(i => i.Value)
					.IsUnique();
			});

			builder.Property(b => b.Description)
				.HasMaxLength(2000);

			builder.Property(b => b.Pages)
				.IsRequired();

			builder.Property(b => b.Language)
				.IsRequired()
				.HasConversion<string>();

			builder.Property(b => b.CoverType)
				.IsRequired()
				.HasConversion<string>();

			builder.Property(b => b.PublicationYear)
				.IsRequired();

			builder.Property(b => b.TotalCopies)
				.IsRequired();

			builder.Property(b => b.AvailableCopies)
				.IsRequired();

			builder.HasOne(b => b.Author)
				.WithMany(a => a.Books)
				.HasForeignKey(b => b.AuthorId);

			builder.HasOne(b => b.Publisher)
				.WithMany(p => p.Books)
				.HasForeignKey(b => b.PublisherId);

			builder.HasMany(b => b.Borrowings)
				.WithOne(br => br.Book)
				.HasForeignKey(br => br.BookId);
		}
	}
}
