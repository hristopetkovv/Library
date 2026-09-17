namespace Library.Application.Books.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReviewCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.GetByIdForUpdateAsync(command.BookId, cancellationToken, r => r.Reviews);
            if (book is null)
                throw new NotFoundException(ValidationMessages.BookNotFound);

            book.RemoveReview(command.ReviewId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
