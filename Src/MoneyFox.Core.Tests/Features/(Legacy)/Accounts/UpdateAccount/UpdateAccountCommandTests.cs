namespace MoneyFox.Core.Tests.Features._Legacy_.Accounts.UpdateAccount;

using Core.Features._Legacy_.Accounts.UpdateAccount;
using Domain.Aggregates.AccountAggregate;
using Microsoft.EntityFrameworkCore;

public class UpdateCategoryCommandTests : InMemoryTestBase
{
    private readonly UpdateAccount.Handler handler;

    public UpdateCategoryCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNameLoaded()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(account);
        await Context.SaveChangesAsync();

        // Act
        account.Change("foo");
        await handler.Handle(command: new(Id: account.Id, Name: account.Name, Note: account.Note, IsExcluded: account.IsExcluded), cancellationToken: default);
        var loadedAccount = await Context.Accounts.SingleAsync(a => a.Id == account.Id);

        // Assert
        loadedAccount.Name.Should().Be("foo");
    }
}
