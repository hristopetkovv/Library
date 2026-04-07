namespace Library.Application.Publishers.Queries.GetAllPublishers
{
	public class GetAllPublishersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllPublishersQuery, List<PublisherListDto>>
	{
		public async Task<List<PublisherListDto>> Handle(GetAllPublishersQuery query, CancellationToken cancellationToken)
		{
			var publishers = await unitOfWork.Publishers.GetAllWithBooksAsync(cancellationToken);

			return [.. publishers.Select(a => new PublisherListDto(
				a.Id,
				a.Name,
				a.Books.Count
			))];
		}
	}
}
