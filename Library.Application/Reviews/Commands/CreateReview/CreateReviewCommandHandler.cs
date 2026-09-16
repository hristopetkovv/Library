namespace Library.Application.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) : IRequestHandler<CreateReviewCommand, Unit>
    {
        public async Task<Unit> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdAsync(command.BookId, cancellationToken);
            if (book is null)
                throw new NotFoundException(ValidationMessages.BookNotFound);

            var review = Review.Create(command.BookId, userContext.UserId, command.Content, command.Rating);
            await unitOfWork.Reviews.AddAsync(review, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
