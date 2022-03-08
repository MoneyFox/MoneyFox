namespace MoneyFox.ViewModels.Accounts
{
    using AutoMapper;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Interfaces;
    using Core._Pending_.Common.Messages;
    using Core.Commands.Accounts.DeleteAccountById;
    using Core.Queries.Accounts.GetAccountEndOfMonthBalance;
    using Core.Queries.Accounts.GetAccounts;
    using Core.Resources;
    using Extensions;
    using Groups;
    using MediatR;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Views.Accounts;
    using Xamarin.Forms;

    public class AccountListViewModel : ObservableRecipient
    {
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts =
            new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

        private bool isRunning;

        public AccountListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            private set
            {
                accounts = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoToAddAccountCommand => new RelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand<AccountViewModel> GoToEditAccountCommand
            => new RelayCommand<AccountViewModel>(
                async accountViewModel
                    => await Shell.Current.Navigation.PushModalAsync(
                        new NavigationPage(new EditAccountPage(accountViewModel.Id))
                        {
                            BarBackgroundColor = Color.Transparent
                        }));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(
                async accountViewModel
                    => await Shell.Current.GoToAsync(
                        $"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));

        public RelayCommand<AccountViewModel> DeleteAccountCommand
            => new RelayCommand<AccountViewModel>(
                async accountViewModel
                    => await DeleteAccountAsync(accountViewModel));

        protected override void OnActivated()
            => Messenger.Register<AccountListViewModel, ReloadMessage>(this, (r, m) => r.OnAppearingAsync());

        protected override void OnDeactivated() => Messenger.Unregister<ReloadMessage>(this);

        public async Task OnAppearingAsync()
        {
            try
            {
                if(isRunning)
                {
                    return;
                }

                isRunning = true;

                var accountVms = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
                accountVms.ForEach(
                    async x =>
                        x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));

                var includedAccountGroup =
                    new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
                var excludedAccountGroup =
                    new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);

                includedAccountGroup.AddRange(accountVms.Where(x => !x.IsExcluded));
                excludedAccountGroup.AddRange(accountVms.Where(x => x.IsExcluded));

                var newAccountCollection = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

                if(includedAccountGroup.Any())
                {
                    newAccountCollection.Add(includedAccountGroup);
                }

                if(excludedAccountGroup.Any())
                {
                    newAccountCollection.Add(excludedAccountGroup);
                }

                // Don't clear and add items separately since iOS doesn't handle batch updates correctly.
                Accounts = newAccountCollection;
            }
            finally
            {
                isRunning = false;
            }

            IsActive = true;
        }

        private async Task DeleteAccountAsync(AccountViewModel accountViewModel)
        {
            if(await dialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeleteAccountConfirmationMessage,
                   Strings.YesLabel,
                   Strings.NoLabel))
            {
                await mediator.Send(new DeactivateAccountByIdCommand(accountViewModel.Id));
                await OnAppearingAsync();
            }
        }
    }
}