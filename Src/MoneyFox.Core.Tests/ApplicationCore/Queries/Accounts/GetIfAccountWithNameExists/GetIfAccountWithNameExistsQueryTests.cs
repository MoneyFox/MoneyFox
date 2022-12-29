namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetIfAccountWithNameExists;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetIfAccountWithNameExistsQueryTests
{
    private readonly AppDbContext context;
    private readonly GetIfAccountWithNameExistsQuery.Handler handler;

    public GetIfAccountWithNameExistsQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Theory]
    [InlineData("Foo", true)]
    [InlineData("foo", true)]
    [InlineData("Foo212", false)]
    public async Task GetExcludedAccountQuery_CorrectNumberLoaded(string name, bool expectedResult)
    {
        // Arrange
        var accountExcluded = new Account(name: "Foo", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(accountName: name, accountId: 0), cancellationToken: default);

        // Assert
        result.Should().Be(expectedResult);
    }
}
