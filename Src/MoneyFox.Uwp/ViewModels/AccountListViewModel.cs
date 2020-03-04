using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccount;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using NLog;

namespace MoneyFox.Uwp.ViewModels
{
    public class AccountListViewModel : ViewModelBase, IAccountListViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly NavigationService navigationService;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountListViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IBalanceCalculationService balanceCalculationService,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    NavigationService navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.settingsFacade = settingsFacade;

            BalanceViewModel = new BalanceViewModel(balanceCalculationService);
            ViewActionViewModel = new AccountListViewActionViewModel(mediator, this.navigationService);

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
        public List<string> MenuActions => new List<string> {Strings.EditLabel, Strings.DeleteLabel};

        public AsyncCommand LoadDataCommand => new AsyncCommand(LoadAsync);

        public RelayCommand<AccountViewModel> OpenOverviewCommand => new RelayCommand<AccountViewModel>(GoToPaymentOverView);

        public RelayCommand<AccountViewModel> EditAccountCommand => new RelayCommand<AccountViewModel>(EditAccount);

        public AsyncCommand<AccountViewModel> DeleteAccountCommand => new AsyncCommand<AccountViewModel>(DeleteAsync);

        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        private void EditAccount(AccountViewModel accountViewModel)
        {
            navigationService.Navigate(ViewModelLocator.EditAccount, accountViewModel.Id);
        }

        private async Task LoadAsync()
        {
            try
            {
                await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();

                var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
                includedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetIncludedAccountQuery())));

                var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
                excludedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetExcludedAccountQuery())));

                Accounts.Clear();

                if (includedAlphaGroup.Any()) Accounts.Add(includedAlphaGroup);
                if (excludedAlphaGroup.Any()) Accounts.Add(excludedAlphaGroup);

                RaisePropertyChanged(nameof(HasNoAccounts));
            }
            catch (Exception ex)
            {
                logManager.Error(ex);
                await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, ex.ToString());
            }
        }

        private void GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null) return;

            navigationService.Navigate(ViewModelLocator.PaymentList, accountViewModel.Id);
        }

        private async Task DeleteAsync(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null) return;

            if (await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeleteAccountByIdCommand(accountToDelete.Id));
                logManager.Info("Account with Id {id} deleted.", accountToDelete.Id);

                Accounts.Clear();
                await LoadAsync();

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }
        }

        private void GoToAddAccount()
        {
            navigationService.Navigate(ViewModelLocator.AddAccount);
        }
    }
}
