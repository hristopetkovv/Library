namespace Library.Application.Publishers.Queries.GetPublisherById
{
	public record GetPublisherByIdQuery(int Id) : IRequest<PublisherDetailDto>;
}
