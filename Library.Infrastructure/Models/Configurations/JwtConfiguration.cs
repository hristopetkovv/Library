namespace Library.Infrastructure.Models.Configurations
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
