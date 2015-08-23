using System;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Logic
{
    public class TransactionTypeLogic
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