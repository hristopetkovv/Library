namespace Library.Application.Authors.Queries.GetAllAuthors
{
	public class GetAllAuthorsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllAuthorsQuery, List<AuthorListDto>>
	{
		public async Task<List<AuthorListDto>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
		{
			var authors = await unitOfWork.Authors.GetAllWithBooksAsync(cancellationToken);

			return [.. authors.Select(a => new AuthorListDto(
				a.Id,
				a.Name,
				a.Books.Count
			))];
		}
	}
}
