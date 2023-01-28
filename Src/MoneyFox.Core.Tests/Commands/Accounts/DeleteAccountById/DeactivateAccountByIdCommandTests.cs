namespace MoneyFox.Core.Tests.Commands.Accounts.DeleteAccountById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Accounts.DeleteAccountById;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class DeactivateAccountByIdCommandTests
{
    private readonly AppDbContext context;
    private readonly DeactivateAccountByIdCommand.Handler handler;

    public DeactivateAccountByIdCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task DeactivatedAccountNotDeleted()
    {
        // Arrange
        var account = new Account("test");
        await context.AddAsync(account);
        await context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(account.Id), cancellationToken: default);

        // Assert
        (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).Should().NotBeNull();
    }

    [Fact]
    public async Task AccountDeactivated()
    {
        // Arrange
        var account = new Account("test");
        await context.AddAsync(account);
        await context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(account.Id), cancellationToken: default);

        // Assert
        (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).IsDeactivated.Should().BeTrue();
    }
}
