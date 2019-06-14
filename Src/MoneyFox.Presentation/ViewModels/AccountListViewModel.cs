using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using NLog;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AccountListViewModel : BaseViewModel, IAccountListViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly INavigationService navigationService;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountListViewModel(ICrudServicesAsync crudService,
                                    IBalanceCalculationService balanceCalculationService,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    INavigationService navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.settingsFacade = settingsFacade;

            BalanceViewModel = new BalanceViewModel(balanceCalculationService);
            ViewActionViewModel = new AccountListViewActionViewModel(crudService, this.navigationService);

            Accounts = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();
        }
        
        public IBalanceViewModel BalanceViewModel { get; }

        public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            private set
            {
                if (accounts == value) return;
                accounts = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasNoAccounts));
            }
        }

        public bool HasNoAccounts => !Accounts.Any();

        public RelayCommand LoadDataCommand => new RelayCommand(Load);

        public RelayCommand<AccountViewModel> OpenOverviewCommand => new RelayCommand<AccountViewModel>(GoToPaymentOverView);

        public RelayCommand<AccountViewModel> EditAccountCommand => new RelayCommand<AccountViewModel>(EditAccount);

        public RelayCommand<AccountViewModel> DeleteAccountCommand => new RelayCommand<AccountViewModel>(Delete);

        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);


        private async Task EditAccount(AccountViewModel accountViewModel)
        {
            await navigationService.Navigate<EditAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter(accountViewModel.Id));
        }

        private async Task Load()
        {
            try
            {
                BalanceViewModel.UpdateBalanceCommand.Execute(null);

                IOrderedQueryable<AccountViewModel> accountViewModels = crudService.ReadManyNoTracked<AccountViewModel>()
                                                                                   .OrderBy(x => x.Name);

                var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
                includedAlphaGroup.AddRange(accountViewModels.AreNotExcluded());

                var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
                excludedAlphaGroup.AddRange(accountViewModels.AreExcluded());

                Accounts.Clear();

                if (includedAlphaGroup.Any())
                {
                    Accounts.Add(includedAlphaGroup);
                }

                if (excludedAlphaGroup.Any())
                {
                    Accounts.Add(excludedAlphaGroup);
                }

                RaisePropertyChanged(nameof(HasNoAccounts));
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, ex.ToString());
            }
        }

        private async Task GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null) return;

            await navigationService.Navigate<PaymentListViewModel, PaymentListParameter>(new PaymentListParameter(accountViewModel.Id));
        }

        private async Task Delete(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null) return;

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage)
                )
            {
                await crudService.DeleteAndSaveAsync<Account>(accountToDelete.Id);
                logManager.Info("Account with Id {id} deleted.", accountToDelete.Id);

                Accounts.Clear();
                await Load();

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }
        }

        private async Task GoToAddAccount()
        {
            await navigationService.Navigate<AddAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter());
        }
    }
}