namespace Library.Application.Books.Commands.RemoveBookFromFavorites
{
    public class RemoveBookFromFavoritesCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<RemoveBookFromFavoritesCommand, Unit>
    {
        public async Task<Unit> Handle(RemoveBookFromFavoritesCommand command, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.BookId, cancellationToken, r => r.UserFavorites);
            if (book is null)
                throw new NotFoundException(ValidationMessages.BookNotFound);

            book.RemoveUserFavorite(userContext.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
