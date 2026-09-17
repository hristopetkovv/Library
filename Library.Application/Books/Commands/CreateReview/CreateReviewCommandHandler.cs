namespace Library.Application.Books.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<CreateReviewCommand, Unit>
    {
        public async Task<Unit> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.BookId, cancellationToken, r => r.Reviews);
            if (book is null)
                throw new NotFoundException(ValidationMessages.BookNotFound);

            var review = Review.Create(command.BookId, userContext.UserId, command.Content, command.Rating);

            book.AddReview(review);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
