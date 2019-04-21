using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.QueryObject;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListViewModel : RouteableViewModelBase
    {
        ObservableAsPropertyHelper<bool> hasNoAccounts;

        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        private ReadOnlyObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts;
        private SourceList<AlphaGroupListGroupCollection<AccountViewModel>> accountsSource;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountListViewModel(IScreen hostScreen,
                                    ICrudServicesAsync crudService = null,
                                    IDialogService dialogService = null,
                                    ISettingsFacade settingsFacade = null)
        {
            HostScreen = hostScreen;

            this.crudService = crudService ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            
            this.WhenActivated(disposables =>
            {
                GoToPaymentViewCommand = ReactiveCommand.Create<AccountViewModel, Unit>(GoToPaymentOverView).DisposeWith(disposables);
                EditAccountCommand = ReactiveCommand.Create<AccountViewModel, Unit>(EditAccount).DisposeWith(disposables);
                AddAccountCommand = ReactiveCommand.Create(GoToAddAccount).DisposeWith(disposables);
                DeleteAccountCommand = ReactiveCommand.CreateFromTask<AccountViewModel, Unit>(DeleteAccount).DisposeWith(disposables);


                LoadAccounts();

                accountsSource.Connect()
                              .ObserveOn(RxApp.MainThreadScheduler)
                              .StartWithEmpty()
                              .Bind(out accounts)
                              .Subscribe()
                              .DisposeWith(disposables);

                hasNoAccounts = this.WhenAnyValue(x => x.accountsSource.Items)
                                    .Select(x => !x.Any())
                                    .ToProperty(this, x => x.HasNoAccounts);


            });
        }
        
        public override string UrlPathSegment => "AccountList";
        public override IScreen HostScreen { get; }

        public ReadOnlyObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts => accounts;

        public bool HasNoAccounts => hasNoAccounts.Value;
        public ReactiveCommand<AccountViewModel, Unit> GoToPaymentViewCommand { get; set; }
        public ReactiveCommand<AccountViewModel, Unit> EditAccountCommand { get; set; }
        public ReactiveCommand<AccountViewModel, Unit> DeleteAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddAccountCommand { get; set; }

        private void LoadAccounts()
        {
            accountsSource = new SourceList<AlphaGroupListGroupCollection<AccountViewModel>>();

            IOrderedQueryable<AccountViewModel> accountViewModels = crudService.ReadManyNoTracked<AccountViewModel>()
                .OrderBy(x => x.Name);

            var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
            includedAlphaGroup.AddRange(accountViewModels.AreNotExcluded());

            var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
            excludedAlphaGroup.AddRange(accountViewModels.AreExcluded());

            accountsSource.Clear();

            if (includedAlphaGroup.Any()) {
                accountsSource.Add(includedAlphaGroup);
            }

            if (excludedAlphaGroup.Any()) {
                accountsSource.Add(excludedAlphaGroup);
            }
        }
        
        private Unit GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null) return new Unit();
            HostScreen.Router.Navigate.Execute(new PaymentListViewModel(HostScreen));
            return new Unit();
        }

        private async Task<Unit> DeleteAccount(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null) return new Unit();

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await crudService.DeleteAndSaveAsync<Account>(accountToDelete.Id);

                accountsSource.Clear();
                LoadAccounts();

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }

            return new Unit();
        }

        private Unit EditAccount(AccountViewModel accountViewModel) {
            HostScreen.Router.Navigate.Execute(new EditAccountViewModel(HostScreen, accountViewModel.Id));
            return new Unit();
        }

        private Unit GoToAddAccount()
        {
            HostScreen.Router.Navigate.Execute(new AddAccountViewModel(HostScreen));
            return new Unit();
        }
    }
}