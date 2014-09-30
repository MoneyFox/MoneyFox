using System;

namespace MoneyManager.Src
{
    //public class RecurringTransactionHelper
    //{
    //   public static void CheckForRecurringTransactions()
    //    {
    //        RecurringTransactionData.LoadList();
    //        CheckIfIntervallIsReady();
    //    }

    //    private static void CheckIfIntervallIsReady()
    //    {
    //        var transactions = TransactionData.LoadRecurringList();

    //        foreach (var recTrans in AllRecurringTransactions)
    //        {
    //            var relTransaction = transactions.Where(x => x.ReccuringTransactionId == recTrans.Id)
    //                .OrderBy(x => x.Date).Last();

    //            if (CheckIfRepeatable(recTrans, relTransaction))
    //            {
    //                SaveTransaction(recTrans);
    //            }
    //        }
    //    }

    //    private static bool CheckIfRepeatable(RecurringTransaction recTrans, FinancialTransaction relTransaction)
    //    {
    //        switch (recTrans.Recurrence)
    //        {
    //            case (int)TransactionRecurrence.Weekly:
    //                return (relTransaction.Date - DateTime.Now).Days >= 7;

    //            case (int)TransactionRecurrence.Monthly:
    //                return relTransaction.Date.Month != DateTime.Now.Month;
    //        }
    //        return false;
    //    }

    //    private static void SaveTransaction(RecurringTransaction recurringTransaction)
    //    {
    //        var newTransaction = new FinancialTransaction
    //        {
    //            ChargedAccountId = recurringTransaction.ChargedAccountId,
    //            Date = DateTime.Now,
    //            IsRecurring = true,
    //            Amount = recurringTransaction.Amount,
    //            Currency = recurringTransaction.Currency,
    //            CategoryId = recurringTransaction.CategoryId,
    //            Type = recurringTransaction.Type,
    //            ReccuringTransactionId = recurringTransaction.Id,
    //            Note = recurringTransaction.Note,
    //        };

    //        TransactionData.Save(newTransaction);
    //    }
    }
}
