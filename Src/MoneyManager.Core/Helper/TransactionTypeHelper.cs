using System;
using MoneyManager.Foundation;

namespace MoneyManager.Core.Helper
{
    /// <summary>
    ///     A collection of helper methods for handling TransactionTypes
    /// </summary>
    public class TransactionTypeHelper
    {
        /// <summary>
        ///     Parse a string to TransactionType
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>Parsed Transactiontype.</returns>
        public static TransactionType GetEnumFromString(string input)
        {
            return (TransactionType) Enum.Parse(typeof (TransactionType), input);
        }

        /// <summary>
        ///     Returns based on an enum int the title for the transaction type.
        /// </summary>
        /// <param name="type">Int of the enum.</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(int type)
        {
            return GetViewTitleForType((TransactionType) type);
        }

        /// <summary>
        ///     Returns based on an transaction type the title.
        /// </summary>
        /// <param name="type">Transactiontype for which the title is searched.</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Spending:
                    return Strings.SpendingTitle;

                case TransactionType.Income:
                    return Strings.IncomeTitle;

                case TransactionType.Transfer:
                    return Strings.TransferTitle;

                default:
                    return string.Empty;
            }
        }
    }
}