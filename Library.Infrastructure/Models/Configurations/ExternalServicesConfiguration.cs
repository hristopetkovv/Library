namespace Library.Infrastructure.Models.Configurations
{
    public class ExternalServicesConfiguration
    {
        public const string SectionName = "ExternalServices";

        public required string GoogleBooksApiBaseUrl { get; set; }
        public required string GoogleBooksApiKey { get; set; }
        public required string OpenLibraryApiBaseUrl { get; set; }
        public required string OpenLibraryApiCoverUrl { get; set; }
    }
}
