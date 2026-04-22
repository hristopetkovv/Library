namespace Library.Contracts.Authors
{
	public record UpdateAuthorRequest(
		string Name,
		string? Biography
	);
}
