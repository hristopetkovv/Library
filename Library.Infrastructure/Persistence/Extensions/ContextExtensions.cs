namespace Library.Infrastructure.Persistence.Extensions
{
    public static class ContextExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

        public static async Task SeedAsync(
            LibraryDbContext context,
            IFileStorageService fileStorageService,
            IHttpClientFactory httpClientFactory,
            ILogger<LibraryDbContext> logger
        )
        {
            await context.Database.MigrateAsync();

            // Check if already seeded
            if (await context.Users.AnyAsync())
                return;

            logger.LogInformation("[Seed] Starting database seeding...");

            await context.Database.BeginTransactionAsync();

            var dateTimeNow = DateTime.UtcNow;

            // Seed Admin User
            var user = await SeedHelpers.SeedAdminAsync(context, dateTimeNow);
            logger.LogInformation("[Seed] Admin user created.");

            // Seed Genres
            var genres = await SeedHelpers.SeedGenresAsync(context, user.Id, dateTimeNow);
            logger.LogInformation($"[Seed] {genres.Count} genres created.");

            // Seed Authors
            var authors = await SeedHelpers.SeedAuthorsAsync(context, user.Id, dateTimeNow);
            logger.LogInformation($"[Seed] {authors.Count} authors created.");

            // Seed Publishers
            var publishers = await SeedHelpers.SeedPublishersAsync(context, user.Id, dateTimeNow);
            logger.LogInformation($"[Seed] {publishers.Count} publishers created.");

            // Seed Books
            await SeedHelpers.SeedBooksAsync(
                context, authors, publishers, genres,
                fileStorageService, httpClientFactory,
                user.Id, dateTimeNow
            );

            await context.Database.CommitTransactionAsync();
            logger.LogInformation("Database seeding completed.");

        }
    }
}
