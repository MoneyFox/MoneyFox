namespace MoneyFox.Ui.Common.Utilities;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Resources;

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
    {
        return (PaymentType)Enum.Parse(enumType: typeof(PaymentType), value: input);
    }

    /// <summary>
    ///     Returns based on an enum int the title for the PaymentType.
    /// </summary>
    /// <param name="type">Int of the enum.</param>
    /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
    /// <returns>Title for the enum.</returns>
    public static string GetViewTitleForType(int type, bool isEditMode)
    {
        return GetViewTitleForType(type: (PaymentType)type, isEditMode: isEditMode);
    }

    /// <summary>
    ///     Returns based on an PaymentType the title.
    /// </summary>
    /// <param name="type">PaymentType for which the title is searched.</param>
    /// <param name="isEditMode">States if the title is used for the edit mode or for adding</param>
    /// <returns>Title for the enum.</returns>
    public static string GetViewTitleForType(PaymentType type, bool isEditMode)
    {
        return type switch
        {
            PaymentType.Expense => isEditMode ? Strings.EditSpendingTitle : Strings.AddExpenseTitle,
            PaymentType.Income => isEditMode ? Strings.EditIncomeTitle : Strings.AddIncomeTitle,
            PaymentType.Transfer => isEditMode ? Strings.EditTransferTitle : Strings.AddTransferTitle,
            _ => string.Empty
        };
    }

    /// <summary>
    ///     Determines the string for PaymentType based on the passed int.
    /// </summary>
    /// <param name="type">The PaymentViewModel type as int.</param>
    /// <returns>The string for the determined type.</returns>
    public static string GetTypeString(int type)
    {
        return type switch
        {
            (int)PaymentType.Income => PaymentType.Income.ToString(),
            (int)PaymentType.Expense => PaymentType.Expense.ToString(),
            (int)PaymentType.Transfer => PaymentType.Transfer.ToString(),
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(type), message: "Passed Number didn't match to a payment type.")
        };
    }
}
