namespace MoneyFox.Ui.Views.Ledgers.LedgerModification;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Core.Features.LedgerCreation;
using Domain;
using MediatR;

internal sealed class AddLedgerViewModel : BasePageViewModel
{
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;
    private Money currentBalance;
    private bool isExcluded;

    private string name = string.Empty;
    private string note = string.Empty;

    public AddLedgerViewModel(IMediator mediator, INavigationService navigationService, ISettingsFacade settingsFacade)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;
        currentBalance = Money.Zero(settingsFacade.DefaultCurrency);
    }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public Money CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(field: ref currentBalance, newValue: value);
    }

    public string Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set => SetProperty(field: ref isExcluded, newValue: value);
    }

    public bool IsValid => IsBusy is false && string.IsNullOrEmpty(Name) is false;

    public AsyncRelayCommand SaveCommand => new(execute: SaveAccountAsync, canExecute: () => IsValid);

    private async Task SaveAccountAsync()
    {
        IsBusy = true;
        try
        {
            await mediator.Send(new CreateLedger.Command(name: Name, currentBalance: CurrentBalance, note: Note, isExcludeFromEndOfMonthSummary: IsExcluded));
            await navigationService.NavigateBackAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
