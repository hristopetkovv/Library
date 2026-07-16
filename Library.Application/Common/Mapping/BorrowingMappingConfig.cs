namespace Library.Application.Common.Mapping
{
    public static class BorrowingMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Borrowing, BorrowingBasicDto>
                .NewConfig()
                .Map(dest => dest.BookTitle, src => src.Book.Title);

            TypeAdapterConfig<Borrowing, BorrowingDetailDto>
                .NewConfig()
                .Map(dest => dest.UserEmail, src => src.User.Email);
        }
    }
}
