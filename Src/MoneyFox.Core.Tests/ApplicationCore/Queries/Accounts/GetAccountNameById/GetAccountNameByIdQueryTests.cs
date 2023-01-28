namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountNameById;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;

public class GetAccountNameByIdQueryTests : InMemoryTestBase
{
    private readonly GetAccountNameByIdQuery.Handler handler;

    public GetAccountNameByIdQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetAccountByIdQuery_CorrectNumberLoaded()
    {
        // Arrange
        var account1 = new Account(name: "test2", initialBalance: 80);
        await Context.AddAsync(account1);
        await Context.SaveChangesAsync();

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
