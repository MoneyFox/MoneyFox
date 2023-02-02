namespace MoneyFox.Ui.Views.Accounts;

using System.Collections.ObjectModel;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Queries;
using MediatR;
using Resources.Strings;

internal sealed class AccountListViewModel : BasePageViewModel, IRecipient<ReloadMessage>
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts = new();

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

    public AsyncRelayCommand GoToAddAccountCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand<AccountViewModel> GoToEditAccountCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.EditAccountRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<AccountViewModel> GoToTransactionListCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.PaymentListRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<AccountViewModel> DeleteAccountCommand => new(async avm => await DeleteAccountAsync(avm));

    public async void Receive(ReloadMessage message)
    {
        await InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            var accountVms = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
            accountVms.ForEach(async x => x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));
            var includedAccountGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Translations.IncludedAccountsHeader);
            var excludedAccountGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Translations.ExcludedAccountsHeader);
            includedAccountGroup.AddRange(accountVms.Where(x => !x.IsExcluded));
            excludedAccountGroup.AddRange(accountVms.Where(x => x.IsExcluded));
            var newAccountCollection = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();
            if (includedAccountGroup.Any())
            {
                newAccountCollection.Add(includedAccountGroup);
            }

            if (excludedAccountGroup.Any())
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
    }

    private async Task DeleteAccountAsync(AccountViewModel accountViewModel)
    {
        if (await dialogService.ShowConfirmMessageAsync(
                title: Translations.DeleteTitle,
                message: Translations.DeleteAccountConfirmationMessage,
                positiveButtonText: Translations.YesLabel,
                negativeButtonText: Translations.NoLabel))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(accountViewModel.Id));
            await InitializeAsync();
        }
    }
}
