namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountNameById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class GetAccountNameByIdQueryTests
{
    private readonly AppDbContext context;
    private readonly GetAccountNameByIdQuery.Handler handler;

    public GetAccountNameByIdQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetAccountByIdQuery_CorrectNumberLoaded()
    {
        // Arrange
        var account1 = new Account(name: "test2", initialBalance: 80);
        await context.AddAsync(account1);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(account1.Id), cancellationToken: default);

        // Assert
        result.Should().Be(account1.Name);
    }

    [Fact]
    public async Task EmptyStringWhenNoAccountFound()
    {
        // Act
        var result = await handler.Handle(request: new(33), cancellationToken: default);

        // Assert
        result.Should().Be(string.Empty);
    }
}
