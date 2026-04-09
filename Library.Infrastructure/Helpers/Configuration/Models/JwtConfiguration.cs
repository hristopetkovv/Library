namespace Library.Infrastructure.Helpers.Configuration.Models
{
	public class JwtConfiguration
	{
		public const string SectionName = "JwtConfiguration";

		public required string Audience { get; set; }
		public required string Issuer { get; set; }
		public required string SecretKey { get; set; }
		public required int ValidDays { get; set; }
	}
}
