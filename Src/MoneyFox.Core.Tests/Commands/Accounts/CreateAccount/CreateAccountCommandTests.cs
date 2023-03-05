namespace MoneyFox.Core.Tests.Commands.Accounts.CreateAccount;

using Core.Features._Legacy_.Accounts.CreateAccount;

public class CreateAccountCommandTests : InMemoryTestBase
{
    private readonly CreateAccountCommand.Handler handler;

    public CreateAccountCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        // Act
        await handler.Handle(request: new(name: "test", currentBalance: 80), cancellationToken: default);

        // Assert
        _ = Assert.Single(Context.Accounts);
    }
}
