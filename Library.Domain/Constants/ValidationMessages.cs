namespace Library.Domain.Constants
{
	public class ValidationMessages
	{
		// Author
		public const string AuthorNameRequired = nameof(AuthorNameRequired);
		public const string AuthorNameMaxLength = nameof(AuthorNameMaxLength);
		public const string AuthorBiographyRequired = nameof(AuthorBiographyRequired);
		public const string AuthorBiographyMaxLength= nameof(AuthorBiographyMaxLength);
		public const string AuthorInvalidId= nameof(AuthorInvalidId);
		public const string AuthorHasAssociatedBooks = nameof(AuthorHasAssociatedBooks);
		public const string AuthorNotFound = nameof(AuthorNotFound);
		public const string AuthorWithThatNameExists = nameof(AuthorWithThatNameExists);

		// Book
		public const string BookTitleRequired = nameof(BookTitleRequired);
		public const string BookTitleMaxLength = nameof(BookTitleMaxLength);
		public const string BookISBNRequired = nameof(BookISBNRequired);
		public const string BookISBNInvalidFormat = nameof(BookISBNInvalidFormat);
		public const string BookPagesGreaterThanZero = nameof(BookPagesGreaterThanZero);
		public const string BookPublicationYearInvalid = nameof(BookPublicationYearInvalid);
		public const string BookPublicationYearInvalidMaxYear = nameof(BookPublicationYearInvalidMaxYear);
		public const string BookTotalCopiesNegative = nameof(BookTotalCopiesNegative);
		public const string BookDescriptionMaxLength = nameof(BookDescriptionMaxLength);
		public const string BookGenreRequired = nameof(BookGenreRequired);
		public const string BookInvalidId = nameof(BookInvalidId);
		public const string BookNotFound = nameof(BookNotFound);
		public const string BookHasActiveBorrowings = nameof(BookHasActiveBorrowings);
		public const string BookHasNoAvailableCopies = nameof(BookHasNoAvailableCopies);
		public const string BookAvailableCannotExceedTotalCopies = nameof(BookAvailableCannotExceedTotalCopies);

		// Borrow
		public const string BorrowingInvalidId = nameof(BorrowingInvalidId);
		public const string BorrowingNotFound = nameof(BorrowingNotFound);
		public const string BorrowingBookAlreadyReturned = nameof(BorrowingBookAlreadyReturned);

		// Publisher
		public const string PublisherInvalidId = nameof(PublisherInvalidId);
		public const string PublisherNameRequired = nameof(PublisherNameRequired);
		public const string PublisherNameMaxLength = nameof(PublisherNameMaxLength);
		public const string PublisherNotFound = nameof(PublisherNotFound);
		public const string PublisherWithThatNameExists = nameof(PublisherWithThatNameExists);
		public const string PublisherHasAssociatedBooks = nameof(PublisherHasAssociatedBooks);

		// User
		public const string UserInvalidId = nameof(UserInvalidId);
		public const string UserRoleRequired = nameof(UserRoleRequired);
		public const string UserEmailRequired = nameof(UserEmailRequired);
		public const string UserFirstNameRequired = nameof(UserFirstNameRequired);
		public const string UserFirstNameMaxLength = nameof(UserFirstNameMaxLength);
		public const string UserLastNameRequired = nameof(UserLastNameRequired);
		public const string UserLastNameMaxLength = nameof(UserLastNameMaxLength);
		public const string UserAddressMaxLength = nameof(UserAddressMaxLength);
		public const string UserPhoneNumberMaxLength = nameof(UserPhoneNumberMaxLength);
		public const string UserPasswordRequired = nameof(UserPasswordRequired);
		public const string UserPasswordInvalidRequirements = nameof(UserPasswordInvalidRequirements);
		public const string UserPasswordAgainRequired = nameof(UserPasswordAgainRequired);
		public const string UserPasswordsMissMatch = nameof(UserPasswordsMissMatch);
		public const string UserEmailExists = nameof(UserEmailExists);
		public const string UserEmailOrPasswordInvalid = nameof(UserEmailOrPasswordInvalid);
		public const string UserNotFound = nameof(UserNotFound);
		public const string UserCannotBorrowMore = nameof(UserCannotBorrowMore);
		public const string UserHasOverdueBooks = nameof(UserHasOverdueBooks);
		public const string UserCannotChangeOwnRole = nameof(UserCannotChangeOwnRole);
		public const string UserViewOwnProfileOnly = nameof(UserViewOwnProfileOnly);
		public const string UserAddressRequired = nameof(UserAddressRequired);
		public const string UserPhoneNumberRequired = nameof(UserPhoneNumberRequired);
		public const string UserEmailInvalidFormat = nameof(UserEmailInvalidFormat);
		public const string UserHasActiveBorrowings = nameof(UserHasActiveBorrowings);
		public const string UserAccountLocked = nameof(UserAccountLocked);
		public const string UserAccountInactive = nameof(UserAccountInactive);
		public const string InvalidCurrentPassword = nameof(InvalidCurrentPassword);
    }
}
