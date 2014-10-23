using MoneyManager.Foundation;
using System;

namespace MoneyManager.Src
{
    internal class TransactionTypeHelper
    {
        public static TransactionType GetEnumFromString(string input)
        {
            return (TransactionType) Enum.Parse(typeof (TransactionType), input);
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
                    return Translation.GetTranslation("SpendingTitle");

                case TransactionType.Income:
                    return Translation.GetTranslation("IncomeTitle");

                case TransactionType.Transfer:
                    return Translation.GetTranslation("TransferTitle");

                default:
                    return String.Empty;
            }
        }
    }
}