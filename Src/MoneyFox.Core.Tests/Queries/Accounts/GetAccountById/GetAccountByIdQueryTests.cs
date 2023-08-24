namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountById;

using Core.Queries;
using Domain.Aggregates.AccountAggregate;

public class GetAccountByIdQueryTests : InMemoryTestBase
{
    [Fact]
    public async Task GetAccountByIdQuery_CorrectNumberLoaded()
    {
        // Arrange
        var account1 = new Account(name: "test2", initialBalance: 80);
        var account2 = new Account(name: "test3", initialBalance: 80);
        await Context.AddAsync(account1);
        await Context.AddAsync(account2);
        await Context.SaveChangesAsync();

        // Act
        var result = await new GetAccountByIdQuery.Handler(Context).Handle(request: new(account1.Id), cancellationToken: default);

        // Assert
        result.Name.Should().Be(account1.Name);
    }

    [Fact]
    public async Task ThrowException_WhenAccountNotFound()
    {
        // Act
        var act = () => new GetAccountByIdQuery.Handler(Context).Handle(request: new(1), cancellationToken: default);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
