namespace Library.Application.Common.Mapping
{
    public static class BookMappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Book, BookBasicDto>
                .NewConfig()
                .Map(dest => dest.ISBN, src => src.ISBN.Value);

            TypeAdapterConfig<Book, BookDetailDto>
                .NewConfig()
                .Map(dest => dest.ISBN, src => src.ISBN.Value);

            TypeAdapterConfig<Book, BookListDto>
                .NewConfig()
                .Map(dest => dest.ISBN, src => src.ISBN.Value);

            TypeAdapterConfig<Genre, GenreDto>
                .NewConfig()
                .Map(dest => dest.GenreId, src => src.Id)
                .Map(dest => dest.GenreName, src => src.Name)
                .Map(dest => dest.GenreNameBg, src => src.NameBg)
                .Map(dest => dest.GenreCategory, src => src.Category);
        }
    }
}
