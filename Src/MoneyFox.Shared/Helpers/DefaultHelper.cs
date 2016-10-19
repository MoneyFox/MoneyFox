using System.Collections.Generic;
using System.Linq;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;

namespace MoneyFox.Shared.Helpers
{
    public class DefaultHelper
    {
        /// <summary>
        ///     Returns the default account out of a list.
        ///     Has a dependency to <see cref="ISettingsManager" />
        /// </summary>
        /// <param name="accounts">List of Accounts to filter.</param>
        /// <returns>The as default determined account.</returns>
        public static AccountViewModel GetDefaultAccount(List<AccountViewModel> accounts)
        {
            var settingsManager = Mvx.Resolve<ISettingsManager>();

            if (accounts == null)
            {
                accounts = new List<AccountViewModel>();
            }

            if (accounts.Any() && (settingsManager.DefaultAccount != -1))
            {
                return accounts.FirstOrDefault(x => x.Id == settingsManager.DefaultAccount);
            }

            return accounts.FirstOrDefault();
        }
    }
}