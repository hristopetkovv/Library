namespace Library.Contracts.Authors
{
    public record CreateAuthorRequest(
        string Name,
        string? Biography
    );
}
