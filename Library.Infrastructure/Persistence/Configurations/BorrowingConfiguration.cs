namespace Library.Infrastructure.Persistence.Configurations
{
	public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
	{
		public void Configure(EntityTypeBuilder<Borrowing> builder)
		{
			builder.Property(b => b.BorrowDate)
				.IsRequired();

			builder.Property(b => b.DueDate)
				.IsRequired();

			builder.Property(b => b.Status)
				.IsRequired()
				.HasConversion<string>();

			builder.HasOne(b => b.Book)
				.WithMany(book => book.Borrowings)
				.HasForeignKey(b => b.BookId);

			builder.HasOne(b => b.User)
			   .WithMany(user => user.Borrowings)
			   .HasForeignKey(b => b.UserId);

			builder.HasIndex(b => b.UserId);
			builder.HasIndex(b => b.BookId);
			builder.HasIndex(b => b.Status);
			builder.HasIndex(b => b.DueDate);
		}
	}
}
