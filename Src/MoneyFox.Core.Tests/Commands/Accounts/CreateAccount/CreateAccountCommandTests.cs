namespace MoneyFox.Core.Tests.Commands.Accounts.CreateAccount;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using MoneyFox.Infrastructure.Persistence;
using TestFramework;
using Xunit;

[ExcludeFromCodeCoverage]
public class CreateAccountCommandTests
{
    private readonly AppDbContext context;
    private readonly CreateAccountCommand.Handler handler;

    public CreateAccountCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new CreateAccountCommand.Handler(context);
    }

    [Fact]
    public async Task GetAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        // Act
        _ = await handler.Handle(request: new CreateAccountCommand(name: "test", currentBalance: 80), cancellationToken: default);

        // Assert
        _ = Assert.Single(context.Accounts);
    }
}

