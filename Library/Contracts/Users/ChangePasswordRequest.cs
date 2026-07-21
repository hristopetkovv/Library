namespace Library.Contracts.Users
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
