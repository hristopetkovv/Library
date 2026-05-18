namespace Library.Application.Publishers.Queries.GetAllPublishers
{
	public record GetAllPublishersQuery(string PublisherName) : IRequest<List<PublisherListDto>>;
}
