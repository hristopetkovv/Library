namespace Library.Application.Publishers.Queries.GetAllPublishers
{
	public class GetAllPublishersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllPublishersQuery, List<PublisherListDto>>
	{
		public async Task<List<PublisherListDto>> Handle(GetAllPublishersQuery query, CancellationToken cancellationToken)
		{
			var publishers = await unitOfWork.Publishers.GetAllFilteredAsync(p =>
            string.IsNullOrWhiteSpace(query.PublisherName) || p.Name.ToLower().Contains(query.PublisherName.ToLower()),
                cancellationToken, 
				p => p.Books
			);

			return [.. publishers.Select(p => new PublisherListDto(
				p.Id,
				p.Name,
				p.Books.Count
			))];
		}
	}
}
