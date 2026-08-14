using Library.Infrastructure.Services.Helpers;

namespace Library.Application.Tests.Auth;

public class PasswordHasherTests
{
    private readonly PasswordHasher sut = new();

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyHashAndSalt()
    {
        var hash = sut.HashPassword("Password123!", out var salt);

        hash.Should().NotBeNullOrEmpty();
        salt.Should().NotBeNullOrEmpty();
        hash.Should().NotBe("Password123!");
    }

    [Fact]
    public void HashPassword_ShouldProduceDifferentSaltAndHash_ForDifferentCalls()
    {
        var hash1 = sut.HashPassword("Password123!", out var salt1);
        var hash2 = sut.HashPassword("Password123!", out var salt2);

        salt1.Should().NotBe(salt2);
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
    {
        var hash = sut.HashPassword("Password123!", out var salt);

        sut.VerifyPassword("Password123!", hash, salt).Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        var hash = sut.HashPassword("Password123!", out var salt);

        sut.VerifyPassword("WrongPassword1!", hash, salt).Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "salt", "hash")]
    [InlineData("", "salt", "hash")]
    [InlineData("   ", "salt", "hash")]
    [InlineData("password", null, "hash")]
    [InlineData("password", "", "hash")]
    [InlineData("password", "   ", "hash")]
    [InlineData("password", "salt", null)]
    [InlineData("password", "salt", "")]
    [InlineData("password", "salt", "   ")]
    public void VerifyPassword_ShouldReturnFalse_WhenInputIsNullOrWhiteSpace(string? password, string? salt, string? hashedPassword)
    {
        sut.VerifyPassword(password!, hashedPassword!, salt!).Should().BeFalse();
    }
}
