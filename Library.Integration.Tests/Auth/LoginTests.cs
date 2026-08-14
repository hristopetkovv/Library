namespace Library.Integration.Tests.Auth
{
    public class LoginTests(LibraryWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task Login_ShouldReturnOk_WithToken_WhenCredentialsValid()
        {
            // Arrange
            var user = await SeedUserAsync(email: "member@test.com", role: UserRole.Member, password: "Password123!");

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "member@test.com",
                password = "Password123!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var token = root.GetProperty("token").GetString();
            token.Should().NotBeNullOrEmpty();

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Member");

            root.GetProperty("user").GetProperty("id").GetInt32().Should().Be(user.Id);
            root.GetProperty("user").GetProperty("email").GetString().Should().Be("member@test.com");
            root.GetProperty("user").GetProperty("firstName").GetString().Should().Be("Test");
            root.GetProperty("user").GetProperty("lastName").GetString().Should().Be("User");
            root.GetProperty("user").GetProperty("role").GetInt32().Should().Be((int)UserRole.Member);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenAdminLogsIn()
        {
            // Arrange
            var user = await SeedUserAsync(email: "admin@test.com", role: UserRole.Admin, password: "Password123!");

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "admin@test.com",
                password = "Password123!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(root.GetProperty("token").GetString());
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

            root.GetProperty("user").GetProperty("role").GetInt32().Should().Be((int)UserRole.Admin);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordIncorrect()
        {
            // Arrange
            var user = await SeedUserAsync(email: "member@test.com", password: "Password123!");

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "member@test.com",
                password = "WrongPassword1!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            (await ReadDetailAsync(response)).Should().Be(ValidationMessages.UserEmailOrPasswordInvalid);

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var storedUser = await db.Users.FirstAsync(u => u.Id == user.Id);
            storedUser.FailedLoginAttempts.Should().Be(1);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenEmailDoesNotExist()
        {
            // Arrange
            await SeedUserAsync(email: "existing@test.com", password: "Password123!");

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "nobody@test.com",
                password = "Password123!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            (await ReadDetailAsync(response)).Should().Be(ValidationMessages.UserEmailOrPasswordInvalid);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenAccountLocked()
        {
            // Arrange
            await SeedUserAsync(email: "locked@test.com", password: "Password123!", status: UserStatus.Locked);

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "locked@test.com",
                password = "Password123!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            (await ReadDetailAsync(response)).Should().Be(ValidationMessages.UserAccountLocked);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenAccountInactive()
        {
            // Arrange
            await SeedUserAsync(email: "inactive@test.com", password: "Password123!", status: UserStatus.Inactive);

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "inactive@test.com",
                password = "Password123!"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            (await ReadDetailAsync(response)).Should().Be(ValidationMessages.UserAccountInactive);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenRequestInvalid()
        {
            // Arrange
            await SeedUserAsync(email: "member@test.com", password: "Password123!");

            // Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "",
                password = ""
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static async Task<string> ReadDetailAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            return doc.RootElement.GetProperty("detail").GetString()!;
        }
    }
}
