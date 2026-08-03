namespace Library.Application.Tests.Users;

public static class UserFactory
{
    public static User Create(int id, string email, string firstName, string lastName,
        UserRole role = UserRole.Member, UserStatus status = UserStatus.Active,
        List<Borrowing>? borrowings = null)
    {
        var user = User.Create(
            "salt",
            "hash",
            Email.Create(email),
            role,
            FullName.Create(firstName, lastName),
            ContactInfo.Create("Address", "1234567890"));

        typeof(User)
            .GetProperty(nameof(User.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(user, id);

        if (status != UserStatus.Active)
        {
            typeof(User)
                .GetProperty(nameof(User.Status), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                ?.SetValue(user, status);
        }

        if (borrowings is not null)
        {
            var field = typeof(User).GetField("borrowings", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var list = (List<Borrowing>)field.GetValue(user)!;
                list.AddRange(borrowings);
            }
        }

        return user;
    }

    public static User CreateWithoutContactInfo(int id, string email, string firstName, string lastName,
        UserRole role = UserRole.Member, List<Borrowing>? borrowings = null)
    {
        var user = User.Create(
            "salt",
            "hash",
            Email.Create(email),
            role,
            FullName.Create(firstName, lastName),
            null);

        typeof(User)
            .GetProperty(nameof(User.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(user, id);

        if (borrowings is not null)
        {
            var field = typeof(User).GetField("borrowings", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var list = (List<Borrowing>)field.GetValue(user)!;
                list.AddRange(borrowings);
            }
        }

        return user;
    }
}
