namespace MoneyFox.Core.Tests.Queries.Statistics.GetAccountProgression;

using Core.Common.Settings;
using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;

public class GetAccountProgressionHandlerTests : InMemoryTestBase
{
    private readonly GetAccountProgression.Handler getAccountProgressionHandler;

    public GetAccountProgressionHandlerTests()
    {
        var settingsFacade = Substitute.For<ISettingsFacade>();
        settingsFacade.DefaultCurrency.Returns("CHF");
        getAccountProgressionHandler = new(appDbContext: Context, settingsFacade: settingsFacade);
    }

    [Fact]
    public async Task CalculateCorrectSums()
    {
        // Arrange
        var account = new Account("Foo1");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account)
            });

        Context.Add(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: account.Id, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Value.Should().Be(40);
        result[1].Value.Should().Be(-50);
        result[2].Value.Should().Be(-40);
    }

    [Fact]
    public async Task GetValues_CorrectSums()
    {
        // Arrange
        var account = new Account("Foo1");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account)
            });

        Context.Add(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: account.Id, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Color.Should().Be("#87cefa");
        result[1].Color.Should().Be("#cd3700");
        result[2].Color.Should().Be("#cd3700");
    }

    [Fact]
    public async Task CalculateCorrectSums_AllAccounts()
    {
        // Arrange
        var account1 = new Account("Foo1");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account1),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account1),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account1),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account1)
            });

        Context.Add(account1);
        var account2 = new Account("Foo2");
        Context.AddRange(
            new List<Payment>
            {
                new(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: account2),
                new(date: DateTime.Today, amount: 20, type: PaymentType.Expense, chargedAccount: account2),
                new(date: DateTime.Today.AddMonths(-1), amount: 50, type: PaymentType.Expense, chargedAccount: account2),
                new(date: DateTime.Today.AddMonths(-2), amount: 40, type: PaymentType.Expense, chargedAccount: account2)
            });

        Context.Add(account2);
        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: 0, startDate: DateTime.Today.AddYears(-1), endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Value.Should().Be(80);
        result[1].Value.Should().Be(-100);
        result[2].Value.Should().Be(-80);
    }

    [Fact]
    public async Task CalculateCorrectSums_TransferPayments()
    {
        // Arrange
        var account1 = new Account(name: "Foo1", initialBalance: 100);
        var account2 = new Account(name: "Foo2", initialBalance: 100);
        Context.Add(account1);
        Context.Add(account2);
        Context.AddRange(
            new List<Payment>
            {
                new(
                    date: DateTime.Today,
                    amount: 100,
                    type: PaymentType.Transfer,
                    chargedAccount: account1,
                    targetAccount: account2)
            });

        await Context.SaveChangesAsync();

        // Act
        var result = await getAccountProgressionHandler.Handle(
            request: new(accountId: 0, startDate: DateTime.Today, endDate: DateTime.Today.AddDays(3)),
            cancellationToken: default);

        // Assert
        result[0].Value.Should().Be(0);
    }
}
