namespace Library.Application.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReviewCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
        {
            var review = await unitOfWork.Reviews.GetByIdAsync(command.ReviewId, cancellationToken);
            if (review is null)
                throw new NotFoundException(ValidationMessages.ReviewNotFound);

            unitOfWork.Reviews.Remove(review);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
