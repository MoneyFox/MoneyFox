using System;
using MoneyManager.Foundation;
using MoneyManager.Localization;

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
        /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(int type, bool isEditMode)
        {
            return GetViewTitleForType((TransactionType) type, isEditMode);
        }

        /// <summary>
        ///     Returns based on an transaction type the title.
        /// </summary>
        /// <param name="type">Transactiontype for which the title is searched.</param>
        /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(TransactionType type, bool isEditMode)
        {
            switch (type)
            {
                case TransactionType.Spending:
                    return isEditMode ? Strings.EditSpendingTitle : Strings.AddSpendingTitle;

                case TransactionType.Income:
                    return isEditMode ? Strings.EditIncomeTitle : Strings.AddIncomeTitle;

                case TransactionType.Transfer:
                    return isEditMode ? Strings.EditTransferTitle : Strings.AddTransferTitle;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///     Determines the string for transaction type based on the passed int.
        /// </summary>
        /// <param name="type">The Transaction type as int.</param>
        /// <returns>The string for the determined type.</returns>
        public static string GetTypeString(int type)
        {
            switch (type)
            {
                case (int) TransactionType.Income:
                    return TransactionType.Income.ToString();

                case (int) TransactionType.Spending:
                    return TransactionType.Spending.ToString();

                case (int) TransactionType.Transfer:
                    return TransactionType.Transfer.ToString();

                default:
                    throw new ArgumentOutOfRangeException(nameof(type),
                        "Passed Number didn't match to a transaction type.");
            }
        }
    }
}