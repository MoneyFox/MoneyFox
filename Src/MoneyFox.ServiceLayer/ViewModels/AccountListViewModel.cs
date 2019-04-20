using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData.Binding;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListViewModel : ViewModelBase
    {
        ObservableAsPropertyHelper<bool> hasNoAccounts;

        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountListViewModel(IScreen hostScreen,
                                    ICrudServicesAsync crudService = null,
                                    IBalanceCalculationService balanceCalculationService = null,
                                    IDialogService dialogService = null,
                                    ISettingsFacade settingsFacade = null)
        {
            HostScreen = hostScreen;

            this.crudService = crudService ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            
            BalanceViewModel = new BalanceViewModel(hostScreen, balanceCalculationService ?? Locator.Current.GetService<IBalanceCalculationService>());

            //ViewActionViewModel = new AccountListViewActionViewModel(crudService);

            Accounts = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

            GoToPaymentViewCommand = ReactiveCommand.Create<AccountViewModel, Unit>(GoToPaymentOverView);

            this.WhenActivated(async disposables =>
            {
                await LoadAccounts();

                hasNoAccounts = Accounts
                    .ToObservableChangeSet()
                    .Select(x => !x.Any())
                    .ToProperty(this, x => x.HasNoAccounts)
                    .DisposeWith(disposables);
            });
        }
        
        public BalanceViewModel BalanceViewModel { get; }

        //public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public override string UrlPathSegment => "AccountList";
        public override IScreen HostScreen { get; }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            private set => this.RaiseAndSetIfChanged(ref accounts, value);
        }

        public bool HasNoAccounts => hasNoAccounts.Value;

        public ReactiveCommand<AccountViewModel, Unit> GoToPaymentViewCommand { get; set; }

        private async Task LoadAccounts() 
        {
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();

            IOrderedQueryable<AccountViewModel> accountViewModels = crudService.ReadManyNoTracked<AccountViewModel>()
                .OrderBy(x => x.Name);

            var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
            includedAlphaGroup.AddRange(accountViewModels.AreNotExcluded());

            var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
            excludedAlphaGroup.AddRange(accountViewModels.AreExcluded());

            Accounts.Clear();

            if (includedAlphaGroup.Any()) {
                Accounts.Add(includedAlphaGroup);
            }

            if (excludedAlphaGroup.Any()) {
                Accounts.Add(excludedAlphaGroup);
            }
        }


        private async Task EditAccount(AccountViewModel accountViewModel)
        {
            //await navigationService.Navigate<EditAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter(accountViewModel.Id));
        }


        private Unit GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null) return new Unit();
            HostScreen.Router.Navigate.Execute(new PaymentListViewModel(HostScreen));
            return new Unit();
        }

        private async Task Delete(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null) return;

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage)
                )
            {
                await crudService.DeleteAndSaveAsync<Account>(accountToDelete.Id)
                    ;

                Accounts.Clear();
                await LoadAccounts();

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }
        }

        private async Task GoToAddAccount()
        {
            //await navigationService.Navigate<AddAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter());
        }
    }
}