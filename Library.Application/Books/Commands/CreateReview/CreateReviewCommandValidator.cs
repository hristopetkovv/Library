namespace Library.Application.Books.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(ValidationMessages.ReviewContentRequired)
                .MaximumLength(1000).WithMessage(ValidationMessages.ReviewContentMaxLength);

            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage(ValidationMessages.ReviewRatingRequired);

            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage(ValidationMessages.ReviewBookIdRequired);
        }
    }
}
