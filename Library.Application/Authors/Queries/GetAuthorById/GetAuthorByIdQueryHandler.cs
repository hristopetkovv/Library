namespace Library.Application.Authors.Queries.GetAuthorById
{
	public class GetAuthorByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAuthorByIdQuery, AuthorDetailDto>
	{
		public async Task<AuthorDetailDto> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken)
		{
			var author = await unitOfWork.Authors.GetByIdAsync(query.Id, cancellationToken, a => a.Books);
			if (author is null)
				throw new NotFoundException(nameof(Author), query.Id);

			return author.Adapt<AuthorDetailDto>();
		}
	}
}
