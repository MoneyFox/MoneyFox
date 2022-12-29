namespace MoneyFox.Core.Common.Extensions.QueryObjects;

using System;
using System.Linq;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

/// <summary>
///     Provides Extensions for payments queries.
/// </summary>
public static class PaymentQueryExtensions
{
    /// <summary>
    ///     Adds a filter to a query for payments with type expense
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for expenses.</returns>
    public static IQueryable<Payment> IsExpense(this IQueryable<Payment> query)
    {
        return query.Where(payment => payment.Type == PaymentType.Expense);
    }

    /// <summary>
    ///     Adds a filter to a query for payments with type income
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for incomes.</returns>
    public static IQueryable<Payment> IsIncome(this IQueryable<Payment> query)
    {
        return query.Where(payment => payment.Type == PaymentType.Income);
    }

    /// <summary>
    ///     Adds a filter to a query for payments of a specified payment type
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for the specified payment type.</returns>
    public static IQueryable<Payment> IsPaymentType(this IQueryable<Payment> query, PaymentType paymentType)
    {
        return query.Where(payment => payment.Type == paymentType);
    }

    /// <summary>
    ///     Adds a filter to a query for cleared payments
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for cleared payments.</returns>
    public static IQueryable<Payment> AreCleared(this IQueryable<Payment> query)
    {
        return query.Where(payment => payment.IsCleared);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who are not cleared
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for not cleared payments.</returns>
    public static IQueryable<Payment> AreNotCleared(this IQueryable<Payment> query)
    {
        return query.Where(payment => !payment.IsCleared);
    }

    /// <summary>
    ///     Adds a filter to a query for recurring payments
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for recurring payments.</returns>
    public static IQueryable<Payment> AreRecurring(this IQueryable<Payment> query)
    {
        return query.Where(payment => payment.IsRecurring);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who has a date larger or equals to the passed date.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <param name="date">Date to filter for.</param>
    /// <returns>Query filtered for not cleared payments.</returns>
    public static IQueryable<Payment> HasDateLargerEqualsThan(this IQueryable<Payment> query, DateTime date)
    {
        return query.Where(payment => payment.Date.Date >= date.Date);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who has a date smaller or equals to the passed date.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <param name="date">Date to filter for.</param>
    /// <returns>Query filtered for the date.</returns>
    public static IQueryable<Payment> HasDateSmallerEqualsThan(this IQueryable<Payment> query, DateTime date)
    {
        return query.Where(payment => payment.Date.Date <= date.Date);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who are not Transfers
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query filtered for payments who are not transfers.</returns>
    public static IQueryable<Payment> WithoutTransfers(this IQueryable<Payment> query)
    {
        return query.Where(payment => payment.Type != PaymentType.Transfer);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who has a certain id as charged or target account.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <param name="accountId">AccountId to filter for</param>
    /// <returns>Query filtered for the account id.</returns>
    public static IQueryable<Payment> HasAccountId(this IQueryable<Payment> query, int accountId)
    {
        return query.Where(payment => payment.ChargedAccount!.Id == accountId || payment.TargetAccount != null && payment.TargetAccount.Id == accountId);
    }

    /// <summary>
    ///     Adds a filter to a query for payments who has a certain category assosciated.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <param name="categoryId">CategoryId to filter for</param>
    /// <returns>Query filtered for the category id.</returns>
    public static IQueryable<Payment> HasCategoryId(this IQueryable<Payment> query, int categoryId)
    {
        return query.Where(payment => payment.Category!.Id == categoryId);
    }

    /// <summary>
    ///     Orders a query descending by the date.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Ordered Query.</returns>
    public static IQueryable<Payment> OrderDescendingByDate(this IQueryable<Payment> query)
    {
        return query.OrderByDescending(x => x.Date);
    }
}
