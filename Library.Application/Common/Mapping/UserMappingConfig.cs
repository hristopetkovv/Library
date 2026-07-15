namespace Library.Application.Common.Mapping
{
    public static class UserMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<User, UserListDto>
                .NewConfig()
                .Map(dest => dest.Email, src => src.Email.Value)
                .Map(dest => dest.FullName, src => src.FullName.FullNameString);

            TypeAdapterConfig<User, UserDetailDto>
                .NewConfig()
                .Map(dest => dest.FirstName, src => src.FullName.FirstName)
                .Map(dest => dest.LastName, src => src.FullName.LastName)
                .Map(dest => dest.Email, src => src.Email.Value)
                .Map(dest => dest.Address, src => src.ContactInfo != null ? src.ContactInfo.Address : null)
                .Map(dest => dest.PhoneNumber, src => src.ContactInfo != null ? src.ContactInfo.PhoneNumber : null);
        }
    }
}
