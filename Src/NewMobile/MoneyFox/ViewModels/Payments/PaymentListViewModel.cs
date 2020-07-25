using System.Diagnostics;

namespace MoneyFox.ViewModels.Payments
{
    public class PaymentListViewModel : BaseViewModel
    {

        public void Init(string accountId)
        {
            Debug.Write($"Account Id passed {accountId}");
        }
    }
}
