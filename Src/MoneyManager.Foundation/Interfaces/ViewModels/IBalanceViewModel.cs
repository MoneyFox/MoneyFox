namespace MoneyManager.Foundation.Interfaces.ViewModels
{
    public interface IBalanceViewModel
    {
        double TotalBalance { get; set; }

        double EndOfMonthBalance { get; set; }

        void UpdateBalance(bool isTransactionView = false);
    }
}