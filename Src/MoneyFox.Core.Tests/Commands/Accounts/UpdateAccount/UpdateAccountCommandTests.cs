namespace MoneyFox.Core.Tests.Commands.Accounts.UpdateAccount;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Accounts.UpdateAccount;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class UpdateCategoryCommandTests
{
    private readonly AppDbContext context;
    private readonly UpdateAccountCommand.Handler handler;

    public UpdateCategoryCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNumberLoaded()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        await context.AddAsync(account);
        await context.SaveChangesAsync();

        // Act
        account.Change("foo");
        await new UpdateAccountCommand.Handler(context).Handle(request: new(account), cancellationToken: default);
        var loadedAccount = await context.Accounts.FindAsync(account.Id);

        // Assert
        loadedAccount.Name.Should().Be("foo");
    }
}
