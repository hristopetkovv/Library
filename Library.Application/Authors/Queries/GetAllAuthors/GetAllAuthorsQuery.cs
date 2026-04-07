namespace Library.Application.Authors.Queries.GetAllAuthors
{
	public record GetAllAuthorsQuery : IRequest<List<AuthorListDto>>;
}
