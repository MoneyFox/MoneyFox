namespace MoneyFox.Core.Tests.Commands.Accounts.CreateAccount;

using System.Diagnostics.CodeAnalysis;
using Core.Commands.Accounts.CreateAccount;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class CreateAccountCommandTests
{
    private readonly AppDbContext context;
    private readonly CreateAccountCommand.Handler handler;

    public CreateAccountCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        // Act
        _ = await handler.Handle(request: new(name: "test", currentBalance: 80), cancellationToken: default);

        // Assert
        _ = Assert.Single(context.Accounts);
    }
}
