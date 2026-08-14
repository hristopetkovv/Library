using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Infrastructure.Models.Configurations;
using Library.Infrastructure.Services.Auth;
using Microsoft.Extensions.Options;

namespace Library.Application.Tests.Auth;

public class JwtTokenGeneratorTests
{
    private const string SecretKey = "test-secret-key-must-be-long-enough-32chars";

    private static JwtTokenGenerator CreateGenerator()
    {
        var config = new JwtConfiguration
        {
            Audience = "http://localhost:4200",
            Issuer = "http://localhost:4200",
            SecretKey = SecretKey,
            ValidDays = 7
        };

        return new JwtTokenGenerator(Options.Create(config));
    }

    private static User CreateUser(int id = 1, string email = "test@test.com", UserRole role = UserRole.Member)
    {
        var user = User.Create(
            "salt",
            "hash",
            Email.Create(email),
            role,
            FullName.Create("Test", "User"),
            ContactInfo.Create("Address", "1234567890"));

        typeof(User)
            .GetProperty(nameof(User.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(user, id);

        return user;
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WithExpectedClaims()
    {
        var generator = CreateGenerator();
        var user = CreateUser(id: 42, email: "jane@test.com", role: UserRole.Admin);

        var token = generator.GenerateToken(user);

        token.Should().NotBeNullOrEmpty();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "42");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "42");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "jane@test.com");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
    }

    [Fact]
    public void GenerateToken_ShouldSetIssuerAudienceAndExpiry()
    {
        var generator = CreateGenerator();
        var user = CreateUser();

        var token = generator.GenerateToken(user);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Issuer.Should().Be("http://localhost:4200");
        jwt.Audiences.Should().Contain("http://localhost:4200");
        jwt.ValidTo.Should().BeAfter(DateTime.UtcNow.AddDays(6));
        jwt.ValidTo.Should().BeBefore(DateTime.UtcNow.AddDays(8));
    }
}
