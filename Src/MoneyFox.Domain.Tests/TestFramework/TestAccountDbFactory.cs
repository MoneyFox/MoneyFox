namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.AccountAggregate;

internal static class TestAccountDbFactory
{
    internal static Account CreateDbAccount(this TestData.IAccount account)
    {
        return new Account(name: account.Name, initialBalance: account.CurrentBalance, note: account.Note, isExcluded: account.IsExcluded);
    }
}
