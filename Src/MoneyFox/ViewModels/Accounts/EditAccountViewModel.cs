using System.Diagnostics;

namespace MoneyFox.ViewModels.Accounts
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        public void Init(int accountId)
        {
            Debug.Write($"Account Id passed {accountId}");
        }
    }
}
