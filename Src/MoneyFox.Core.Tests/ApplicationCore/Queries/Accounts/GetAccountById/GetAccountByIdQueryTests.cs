namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class GetAccountByIdQueryTests : IDisposable
{
    private readonly AppDbContext context;

    public GetAccountByIdQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        InMemoryAppDbContextFactory.Destroy(context);
    }

    [Fact]
    public async Task GetAccountByIdQuery_CorrectNumberLoaded()
    {
        // Arrange
        var account1 = new Account(name: "test2", initialBalance: 80);
        var account2 = new Account(name: "test3", initialBalance: 80);
        await context.AddAsync(account1);
        await context.AddAsync(account2);
        await context.SaveChangesAsync();

        // Act
        var result = await new GetAccountByIdQuery.Handler(context).Handle(request: new(account1.Id), cancellationToken: default);

        // Assert
        result.Name.Should().Be(account1.Name);
    }

    [Fact]
    public async Task ThrowException_WhenAccountNotFound()
    {
        // Act
        var act = () => new GetAccountByIdQuery.Handler(context).Handle(request: new(1), cancellationToken: default);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
