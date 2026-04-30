namespace Library.Application.Dtos.Helpers
{
	public record FileUploadDto(Stream Content, string FileName, string ContentType);
}
