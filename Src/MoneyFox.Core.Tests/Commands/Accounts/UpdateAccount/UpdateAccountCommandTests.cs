namespace MoneyFox.Core.Tests.Commands.Accounts.UpdateAccount;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Accounts.UpdateAccount;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class UpdateCategoryCommandTests : InMemoryTestBase
{
    private readonly UpdateAccountCommand.Handler handler;

    public UpdateCategoryCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNumberLoaded()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(account);
        await Context.SaveChangesAsync();

        // Act
        account.Change("foo");
        await handler.Handle(request: new(account), cancellationToken: default);
        var loadedAccount = await Context.Accounts.SingleAsync(a => a.Id == account.Id);

        // Assert
        loadedAccount.Name.Should().Be("foo");
    }
}
