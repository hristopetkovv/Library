namespace Library.Application.Tests.Users.Commands;

public class ForgottenPasswordCommandHandlerTests
{
    private readonly ForgottenPasswordCommandHandler handler = new();

    [Fact]
    public async Task Handle_ShouldReturnUnitValue()
    {
        var command = new ForgottenPasswordCommand("test@test.com");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }
}
