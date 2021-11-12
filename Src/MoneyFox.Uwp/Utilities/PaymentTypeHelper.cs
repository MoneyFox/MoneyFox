using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using System;

namespace MoneyFox.Uwp.Utilities
{
    /// <summary>
    ///     A collection of helper methods for handling PaymentTypes
    /// </summary>
    public static class PaymentTypeHelper
    {
        /// <summary>
        ///     Parse a string to PaymentType
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>Parsed PaymentType.</returns>
        public static PaymentType GetEnumFromString(string input)
            => (PaymentType)Enum.Parse(typeof(PaymentType), input);

        /// <summary>
        ///     Returns based on an enum int the title for the PaymentType.
        /// </summary>
        /// <param name="type">Int of the enum.</param>
        /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(int type, bool isEditMode)
            => GetViewTitleForType((PaymentType)type, isEditMode);

        /// <summary>
        ///     Returns based on an PaymentType the title.
        /// </summary>
        /// <param name="type">PaymentType for which the title is searched.</param>
        /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
        /// <returns>Title for the enum.</returns>
        public static string GetViewTitleForType(PaymentType type, bool isEditMode) => type switch
        {
            PaymentType.Expense => isEditMode ? Strings.EditSpendingTitle : Strings.AddExpenseTitle,
            PaymentType.Income => isEditMode ? Strings.EditIncomeTitle : Strings.AddIncomeTitle,
            PaymentType.Transfer => isEditMode ? Strings.EditTransferTitle : Strings.AddTransferTitle,
            _ => string.Empty
        };

        /// <summary>
        ///     Determines the string for PaymentType based on the passed int.
        /// </summary>
        /// <param name="type">The PaymentViewModel type as int.</param>
        /// <returns>The string for the determined type.</returns>
        public static string GetTypeString(int type) => type switch
        {
            (int)PaymentType.Income => PaymentType.Income.ToString(),
            (int)PaymentType.Expense => PaymentType.Expense.ToString(),
            (int)PaymentType.Transfer => PaymentType.Transfer.ToString(),
            _ => throw new ArgumentOutOfRangeException(
                nameof(type),
                "Passed Number didn't match to a payment type.")
        };
    }
}