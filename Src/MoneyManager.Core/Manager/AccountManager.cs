using MoneyManager.Core.DataAccess;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Manager
{
    public class AccountManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly ModifyAccountViewModel modifyAccountViewModel;
        private readonly SettingDataAccess settings;

        /// <summary>
        ///     Creates an AccountManager object.
        /// </summary>
        /// <param name="modifyAccountViewModel">Instance of <cref="ModifyAccountViewModel"/></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}"/></param>
        /// <param name="settings">Instance of <see cref="SettingDataAccess"/></param>
        public AccountManager(IRepository<Account> accountRepository, ModifyAccountViewModel modifyAccountViewModel, SettingDataAccess settings)
        {
            this.accountRepository = accountRepository;
            this.modifyAccountViewModel = modifyAccountViewModel;
            this.settings = settings;
        }

        /// <summary>
        ///     Prepares everything to create a new account.
        /// </summary>
        public void PrepareCreation()
        {
            accountRepository.Selected = new Account();
            modifyAccountViewModel.IsEdit = false;
        }

    }
}
