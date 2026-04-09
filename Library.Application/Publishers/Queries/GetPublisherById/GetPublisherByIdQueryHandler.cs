namespace Library.Application.Publishers.Queries.GetPublisherById
{
	public class GetPublisherByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPublisherByIdQuery, PublisherDetailDto>
	{
		public async Task<PublisherDetailDto> Handle(GetPublisherByIdQuery query, CancellationToken cancellationToken)
		{
			var publisher = await unitOfWork.Publishers.GetByIdAsync(query.Id, cancellationToken, p => p.Books);
			if (publisher == null)
				throw new NotFoundException(nameof(Publisher), query.Id);

			return publisher.Adapt<PublisherDetailDto>();
		}
	}
}
