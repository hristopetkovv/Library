namespace Library.Infrastructure.Persistence.Configurations
{
	public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
	{
		public void Configure(EntityTypeBuilder<Publisher> builder)
		{
			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.HasMany(p => p.Books)
				.WithOne(b => b.Publisher)
				.HasForeignKey(b => b.PublisherId);
		}
	}
}
