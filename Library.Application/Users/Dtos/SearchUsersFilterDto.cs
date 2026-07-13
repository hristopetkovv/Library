namespace Library.Application.Users.Dtos
{
    public record SearchUsersFilterDto(
        string? Email,
        string? FullName
    )
    {
        public Expression<Func<User, bool>> Predicate()
        {
            return u =>
                (string.IsNullOrEmpty(Email) || u.Email.Value.ToLower().Contains(Email.ToLower()))
                && (string.IsNullOrEmpty(FullName) || u.FullName.FullNameString.ToLower().Contains(FullName.ToLower()));
        }
    }
}
