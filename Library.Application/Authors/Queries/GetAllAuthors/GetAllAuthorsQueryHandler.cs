namespace Library.Application.Authors.Queries.GetAllAuthors
{
	public class GetAllAuthorsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllAuthorsQuery, List<AuthorListDto>>
	{
		public async Task<List<AuthorListDto>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
		{
			var authors = await unitOfWork.Authors.GetAllFilteredAsync(a => 
			string.IsNullOrWhiteSpace(query.AuthorName) || a.Name.ToLower().Contains(query.AuthorName.ToLower()), 
			cancellationToken, 
			a => a.Books
			);

			return [.. authors.Select(a => new AuthorListDto(
				a.Id,
				a.Name,
				a.Books.Count
			))];
		}
	}
}
