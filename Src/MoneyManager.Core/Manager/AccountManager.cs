using MoneyManager.Core.DataAccess;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Manager
{
    public class AccountManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly AddAccountViewModel addAccountViewModel;
        private readonly SettingDataAccess settings;

        /// <summary>
        ///     Creates an AccountManager object.
        /// </summary>
        /// <param name="addAccountViewModel">Instance of <cref="AddAccountViewModel"/></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}"/></param>
        /// <param name="settings">Instance of <see cref="SettingDataAccess"/></param>
        public AccountManager(IRepository<Account> accountRepository, AddAccountViewModel addAccountViewModel, SettingDataAccess settings)
        {
            this.accountRepository = accountRepository;
            this.addAccountViewModel = addAccountViewModel;
            this.settings = settings;
        }

        /// <summary>
        ///     Prepares everything to create a new account.
        /// </summary>
        public void PrepareCreation()
        {
            accountRepository.Selected = new Account
            {
                Currency = settings.DefaultCurrency
            };
            addAccountViewModel.IsEdit = false;
        }

    }
}
