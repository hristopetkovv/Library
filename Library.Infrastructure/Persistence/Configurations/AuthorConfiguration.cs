namespace Library.Infrastructure.Persistence.Configurations
{
	public class AuthorConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.Property(a => a.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(a => a.Biography)
				.IsRequired()
				.HasMaxLength(2000);

			builder.HasMany(a => a.Books)
				.WithOne(b => b.Author)
				.HasForeignKey(b => b.AuthorId);
		}
	}
}
