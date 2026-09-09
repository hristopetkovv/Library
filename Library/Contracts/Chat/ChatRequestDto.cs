namespace Library.Contracts.Chat
{
    public record ChatRequestDto(string Message, string Language = "bg");
}
