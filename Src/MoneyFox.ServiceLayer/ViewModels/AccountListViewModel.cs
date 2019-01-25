using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListViewModel : BaseNavigationViewModel //, IAccountListViewModel
    {
        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;
        private readonly IMvxLogProvider logProvider;
        private readonly IMvxNavigationService navigationService;

        private ObservableCollection<AlphaGroupListGroup<AccountViewModel>> accounts;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountListViewModel(ICrudServicesAsync crudService,
                                    IDialogService dialogService,
                                    IMvxLogProvider logProvider,
                                    IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.logProvider = logProvider;

            //BalanceViewModel = new BalanceViewModel(balanceCalculationManager, logProvider, navigationService);
            ViewActionViewModel = new AccountListViewActionViewModel(crudService, logProvider, navigationService);

            Accounts = new ObservableCollection<AlphaGroupListGroup<AccountViewModel>>();
        }
        
        //public IBalanceViewModel BalanceViewModel { get; }

        public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public ObservableCollection<AlphaGroupListGroup<AccountViewModel>> Accounts
        {
            get => accounts;
            set
            {
                if (accounts == value) return;
                accounts = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasNoAccounts));
            }
        }

        public bool HasNoAccounts => !Accounts.Any();
        
        //public MvxAsyncCommand<AccountViewModel> OpenOverviewCommand => new MvxAsyncCommand<AccountViewModel>(GoToPaymentOverView);

        //public MvxAsyncCommand<AccountViewModel> EditAccountCommand => new MvxAsyncCommand<AccountViewModel>(EditAccount);

        //public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand => new MvxAsyncCommand<AccountViewModel>(Delete);

        public MvxAsyncCommand GoToAddAccountCommand => new MvxAsyncCommand(GoToAddAccount);

        /// <inheritdoc />
        public override async void ViewAppeared()
        {
            await Load();
            await RaisePropertyChanged(nameof(Accounts));
        }

        //private async Task EditAccount(AccountViewModel accountViewModel)
        //{
        //    await navigationService.Navigate<ModifyAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter(accountViewModel.Id));
        //}

        private async Task Load()
        {
            try
            {
                //await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();

                //var includedAccountList = (await accountService.GetNotExcludedAccounts()).ToList();
                //var excludedAccountList = (await accountService.GetExcludedAccounts()).ToList();

                List<AccountViewModel> accounts = await crudService.ReadManyNoTracked<AccountViewModel>().ToListAsync();

                var includedAlphaGroup = new AlphaGroupListGroup<AccountViewModel>(Strings.IncludedAccountsHeader);
                includedAlphaGroup.AddRange(accounts.Where(x => !x.IsExcluded));

                var excludedAlphaGroup = new AlphaGroupListGroup<AccountViewModel>(Strings.ExcludedAccountsHeader);
                excludedAlphaGroup.AddRange(accounts.Where(x => x.IsExcluded));

                Accounts.Clear();

                if (includedAlphaGroup.Any())
                {
                    Accounts.Add(includedAlphaGroup);
                }

                if (excludedAlphaGroup.Any())
                {
                    Accounts.Add(excludedAlphaGroup);
                }

                await RaisePropertyChanged(nameof(HasNoAccounts));
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, ex.ToString());
            }
        }

        //private async Task GoToPaymentOverView(AccountViewModel accountViewModel)
        //{
        //    if (accountViewModel == null) return;

        //    await navigationService.Navigate<PaymentListViewModel, PaymentListParameter>(new PaymentListParameter(accountViewModel.Id));
        //}

        //private async Task Delete(AccountViewModel accountToDelete)
        //{
        //    if (accountToDelete == null)
        //    {
        //        return;
        //    }

        //    if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
        //    {
        //        await accountService.DeleteAccount(accountToDelete.Account);

        //        Accounts.Clear();
        //        await Load();
                
        //        settingsManager.LastDatabaseUpdate = DateTime.Now;
        //    }
        //}

        private async Task GoToAddAccount()
        {
            await navigationService.Navigate<AddAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter());
        }
    }
}