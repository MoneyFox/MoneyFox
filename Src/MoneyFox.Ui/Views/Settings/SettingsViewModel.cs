namespace MoneyFox.Ui.Views.Settings;

using System.Globalization;
using Core.Common.Settings;
using Domain;
using MediatR;
using MoneyFox.Core.Queries;

internal sealed class SettingsViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    private readonly IMediator mediator;

    private CurrencyViewModel selectedCurrency = null!;

    private AccountLiteViewModel? selectedAccount;

    public SettingsViewModel(ISettingsFacade settingsFacade, IMediator mediator)
    {
        this.settingsFacade = settingsFacade;
        this.mediator = mediator;
        AvailableCurrencies = Currencies.GetAll().Select(c => new CurrencyViewModel(c.AlphaIsoCode)).OrderBy(c => c.AlphaIsoCode).ToList();
        var currencyToLoad = string.IsNullOrEmpty(settingsFacade.DefaultCurrency) ? RegionInfo.CurrentRegion.ISOCurrencySymbol : settingsFacade.DefaultCurrency;
        SelectedCurrency = AvailableCurrencies.FirstOrDefault(c => c.AlphaIsoCode == currencyToLoad) ?? AvailableCurrencies[0];
        LoadAccounts().Wait();
        SelectedAccount = AvailableAccounts.FirstOrDefault(x => x.Id == settingsFacade.DefaultAccount);
    }

    public CurrencyViewModel SelectedCurrency
    {
        get => selectedCurrency;

        set
        {
            SetProperty(field: ref selectedCurrency, newValue: value);
            settingsFacade.DefaultCurrency = selectedCurrency.AlphaIsoCode;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<CurrencyViewModel> AvailableCurrencies { get; }

    public AccountLiteViewModel? SelectedAccount
    {
        get => selectedAccount;

        set
        {
            SetProperty(field: ref selectedAccount, newValue: value);
            settingsFacade.DefaultAccount = selectedAccount == null? default : selectedAccount.Id;
            OnPropertyChanged();
        }
    }

    public async Task LoadAccounts()
    {
        var accounts = await mediator.Send(new GetAccountsQuery());
        AvailableAccounts = accounts == null ? new List<AccountLiteViewModel>() : accounts.Select(a => new AccountLiteViewModel(a.Id, a.Name)).ToList();
    }

    public List<AccountLiteViewModel> AvailableAccounts { get; private set; } = new();
}
