using System.Diagnostics;

namespace MoneyFox.ViewModels.Accounts
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        public void Init(string accountId)
        {
            Debug.Write($"Account Id passed {accountId}");
        }
    }
}
