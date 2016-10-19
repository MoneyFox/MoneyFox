using System;
using SQLite.Net.Attributes;

namespace MoneyFox.DataAccess.Models
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    [Table("Payments")]
    internal class Payment
    {
        [PrimaryKey, AutoIncrement, Indexed]
        private int Id { get; set; }

        private int ChargedAccountId { get; set; }
        private int TargetAccountId { get; set; }
        private int? CategoryId { get; set; }
        private DateTime Date { get; set; }
        private double Amount { get; set; }
        private bool IsCleared { get; set; }
        private int Type { get; set; }
        private string Note { get; set; }
        private bool IsRecurring { get; set; }
        private int RecurringPaymentId { get; set; }
        private int CurrentAccountId { get; set; }
    }
}