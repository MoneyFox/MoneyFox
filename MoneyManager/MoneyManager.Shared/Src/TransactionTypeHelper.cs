using System;

namespace MoneyManager.Src
{
    public class TransactionTypeHelper
    {
        public static TransactionType GetEnumFromString(string input)
        {
            return (TransactionType)Enum.Parse(typeof(TransactionType), input);
        }

        public static string GetViewTitleForType(int type)
        {
            return GetViewTitleForType((TransactionType) type);
        }

        public static string GetViewTitleForType(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Spending:
                    return Utilities.GetTranslation("SpendingTitle");

                case TransactionType.Income:
                    return Utilities.GetTranslation("IncomeTitle");

                case TransactionType.Transfer:
                    return Utilities.GetTranslation("TransferTitle");

                default:
                    return String.Empty;
            }
        }
    }
}