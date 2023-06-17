namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.LedgerAggregate;
using Domain.Aggregates.TransactionAggregate;

internal static class TestTransactionDbFactory
{
    internal static Transaction CreateDbTransaction(this TestData.ITransaction transaction)
    {
        return Transaction.Create(
            reference: transaction.Reference,
            transaction.LedgerId,
            type: transaction.Type,
            amount: transaction.Amount,
            bookingDate: transaction.BookingDate,
            categoryId: transaction.CategoryId,
            note: transaction.Note);
    }
}
