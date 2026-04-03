namespace Library.Domain.Common
{
	public abstract class BaseAuditableEntity : IAuditable
	{
		public int CreatedByUserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public int? LastModifiedByUserId { get; set; }
		public DateTime? LastModifiedDate { get; set; }
	}
}
