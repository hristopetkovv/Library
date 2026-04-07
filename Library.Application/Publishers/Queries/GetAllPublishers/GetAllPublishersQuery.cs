namespace Library.Application.Publishers.Queries.GetAllPublishers
{
	public record GetAllPublishersQuery : IRequest<List<PublisherListDto>>;
}
