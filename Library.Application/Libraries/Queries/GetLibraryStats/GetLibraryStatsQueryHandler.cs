namespace Library.Application.Libraries.Queries.GetLibraryStats
{
	public class GetLibraryStatsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLibraryStatsQuery, LibraryStatsDto>
	{
		public async Task<LibraryStatsDto> Handle(GetLibraryStatsQuery request, CancellationToken cancellationToken)
		{
			var booksCount = await unitOfWork.Books.CountAsync(b => b.AvailableCopies > 0, cancellationToken);
			var authorsCount = await unitOfWork.Authors.CountAsync(cancellationToken: cancellationToken);
			var publishersCount = await unitOfWork.Publishers.CountAsync(cancellationToken: cancellationToken);

			return new LibraryStatsDto(booksCount, authorsCount, publishersCount);
		}
	}
}
