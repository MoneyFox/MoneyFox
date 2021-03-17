using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccount;
using MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Src;
using MoneyFox.Uwp.ViewModels.Interfaces;
using MoneyFox.Uwp.ViewModels.Payments;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Accounts
{
    [SuppressMessage("Major Code Smell", "S1200:Classes should not be coupled to too many other classes (Single Responsibility Principle)")]
    public class AccountListViewModel : ViewModelBase, IAccountListViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly NavigationService navigationService;

        private bool isRunning;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

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
            ViewActionViewModel = new AccountListViewActionViewModel(this.navigationService);
        }

        public void Subscribe()
            => MessengerInstance.Register<ReloadMessage>(this, async (m) => await LoadAsync());

        public void Unsubscribe()
            => MessengerInstance.Unregister<ReloadMessage>(this);

        public IBalanceViewModel BalanceViewModel { get; }

        public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            private set
            {
                if(accounts == value)
                {
                    return;
                }

                accounts = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasNoAccounts));
            }
        }

        public bool HasNoAccounts => !Accounts.Any();

        public AsyncCommand LoadDataCommand => new AsyncCommand(LoadAsync);

        public RelayCommand<AccountViewModel> OpenOverviewCommand => new RelayCommand<AccountViewModel>(GoToPaymentOverView);

        public RelayCommand<AccountViewModel> EditAccountCommand => new RelayCommand<AccountViewModel>(EditAccount);

        public AsyncCommand<AccountViewModel> DeleteAccountCommand => new AsyncCommand<AccountViewModel>(DeleteAsync);

        private void EditAccount(AccountViewModel accountViewModel) => navigationService.Navigate<EditAccountViewModel>(accountViewModel.Id);

        private async Task LoadAsync()
        {
            try
            {
                if(isRunning)
                {
                    return;
                }

                isRunning = true;
                await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();

                var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
                includedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetIncludedAccountQuery())));
                includedAlphaGroup.ForEach(async x => x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));

                var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
                excludedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetExcludedAccountQuery())));
                excludedAlphaGroup.ForEach(async x => x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));

                Accounts.Clear();

                if(includedAlphaGroup.Any())
                {
                    Accounts.Add(includedAlphaGroup);
                }
                if(excludedAlphaGroup.Any())
                {
                    Accounts.Add(excludedAlphaGroup);
                }

                RaisePropertyChanged(nameof(HasNoAccounts));
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, ex.ToString());
            }
            finally
            {
                isRunning = false;
            }
        }

        private void GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if(accountViewModel == null)
            {
                return;
            }

            navigationService.Navigate<PaymentListViewModel>(accountViewModel.Id);
        }

        private async Task DeleteAsync(AccountViewModel accountToDelete)
        {
            if(accountToDelete == null)
            {
                return;
            }

            if(await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeactivateAccountByIdCommand(accountToDelete.Id));
                logManager.Info("Account with Id {id} deleted.", accountToDelete.Id);

                Accounts.Clear();
                await LoadAsync();

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }
        }
    }
}
