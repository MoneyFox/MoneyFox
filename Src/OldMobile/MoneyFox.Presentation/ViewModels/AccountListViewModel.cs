using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccount;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.Models;

namespace MoneyFox.Presentation.ViewModels
{
    public class AccountListViewModel : ViewModelBase, IAccountListViewModel
    {
        private const int MENU_RESULT_EDIT_INDEX = 0;
        private const int MENU_RESULT_DELETE_INDEX = 1;

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly INavigationService navigationService;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountListViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IBalanceCalculationService balanceCalculationService,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    INavigationService navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.settingsFacade = settingsFacade;

            BalanceViewModel = new BalanceViewModel(balanceCalculationService);
            ViewActionViewModel = new AccountListViewActionViewModel(mediator, this.navigationService);

            Accounts = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

            MessengerInstance.Register<ReloadMessage>(this, async (m) => await MessageReceived());
        }

        public IBalanceViewModel BalanceViewModel { get; }

        public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            private set
            {
                if(accounts == value)
                    return;
                accounts = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasNoAccounts));
            }
        }

        public bool HasNoAccounts => !Accounts.Any();

        public List<string> MenuActions => new List<string> { Strings.EditLabel, Strings.DeleteLabel };

        public AsyncCommand LoadDataCommand => new AsyncCommand(LoadAsync);

        public RelayCommand<AccountViewModel> OpenOverviewCommand => new RelayCommand<AccountViewModel>(GoToPaymentOverView);

        public Command<MaterialMenuResult> MenuSelectedCommand => new Command<MaterialMenuResult>(MenuSelected);

        public AsyncCommand<AccountViewModel> DeleteAccountCommand => new AsyncCommand<AccountViewModel>(DeleteAsync);

        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "Acts as event handler.>")]
        private async void MenuSelected(MaterialMenuResult menuResult)
        {
            var accountViewModel = menuResult.Parameter as AccountViewModel;

            switch(menuResult.Index)
            {
                case MENU_RESULT_EDIT_INDEX:
                    navigationService.NavigateToModal(ViewModelLocator.EditAccount, accountViewModel.Id);
                    break;

                case MENU_RESULT_DELETE_INDEX:
                    await DeleteAsync(accountViewModel);
                    break;

                default:
                    logManager.Warn("Invalid Index for Menu Selected in Account List. Index: {0}", menuResult.Index);
                    break;
            }
        }

        private async Task MessageReceived()
        {
            logManager.Info("Reload Message received");
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                logManager.Info("Update balance ViewModel");
                await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();

                logManager.Info("Load Account list");
                var includedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
                includedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetIncludedAccountQuery())));

                var excludedAlphaGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);
                excludedAlphaGroup.AddRange(mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetExcludedAccountQuery())));

                Accounts.Clear();

                if(includedAlphaGroup.Any())
                    Accounts.Add(includedAlphaGroup);
                if(excludedAlphaGroup.Any())
                    Accounts.Add(excludedAlphaGroup);

                RaisePropertyChanged(nameof(HasNoAccounts));
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
            }
        }

        private void GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if(accountViewModel == null)
                return;

            navigationService.NavigateTo(ViewModelLocator.PaymentList, accountViewModel.Id);
        }

        private async Task DeleteAsync(AccountViewModel accountToDelete)
        {
            if(accountToDelete == null)
                return;

            if(await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
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
            navigationService.NavigateToModal(ViewModelLocator.AddAccount);
        }
    }
}
