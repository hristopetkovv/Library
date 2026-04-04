namespace Library.Domain.Interfaces.Common
{
	public interface IAuditable
	{
		int CreatedByUserId { get; set; }
		DateTime CreatedDate { get; set; }
		int? LastModifiedByUserId { get; set; }
		DateTime? LastModifiedDate { get; set; }
	}
}