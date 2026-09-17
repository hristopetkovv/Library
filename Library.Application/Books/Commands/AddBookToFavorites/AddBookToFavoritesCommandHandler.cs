namespace Library.Application.Books.Commands.AddBookToFavorites
{
    public class AddBookToFavoritesCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<AddBookToFavoritesCommand, Unit>
    {
        public async Task<Unit> Handle(AddBookToFavoritesCommand command, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.BookId, cancellationToken, r => r.UserFavorites);
            if (book is null)
                throw new NotFoundException(ValidationMessages.BookNotFound);

            var userFavorite = UserFavoriteBook.Create(userContext.UserId, command.BookId);

            book.AddUserFavorite(userFavorite);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
