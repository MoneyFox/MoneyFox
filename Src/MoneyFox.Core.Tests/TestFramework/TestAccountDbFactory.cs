namespace MoneyFox.Core.Tests.TestFramework;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

internal static class TestAccountDbFactory
{
    internal static Account CreateDbAccount(this TestData.IAccount? account)
    {
        return account is null
            ? null
            : new Account(name: account.Name, initialBalance: account.CurrentBalance, note: account.Note, isExcluded: account.IsExcluded);
    }
}
