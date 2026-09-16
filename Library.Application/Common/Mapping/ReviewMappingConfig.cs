namespace Library.Application.Common.Mapping
{
    public class ReviewMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Review, ReviewDto>
                .NewConfig()
                .Map(dest => dest.UserFullName, src => src.User.FullName.FullNameString)
                .Map(dest => dest.CreatedAt, src => src.CreatedDate);
        }
    }
}
