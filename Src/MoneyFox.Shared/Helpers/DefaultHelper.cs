using System.Collections.Generic;
using System.Linq;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Helpers
{
    public class DefaultHelper
    {
        public static Account GetDefaultAccount(List<Account> accounts)
        {
            if (accounts == null)
            {
                accounts = new List<Account>();
            }

            if (accounts.Any() && SettingsHelper.DefaultAccount != -1)
            {
                return accounts.FirstOrDefault(x => x.Id == SettingsHelper.DefaultAccount);
            }

            return accounts.FirstOrDefault();
        }
    }
}