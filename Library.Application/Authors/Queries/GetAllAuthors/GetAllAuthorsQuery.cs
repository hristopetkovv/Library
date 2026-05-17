namespace Library.Application.Authors.Queries.GetAllAuthors
{
	public record GetAllAuthorsQuery(string AuthorName) : IRequest<List<AuthorListDto>>;
}
