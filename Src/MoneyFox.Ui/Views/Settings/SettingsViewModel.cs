namespace MoneyFox.Ui.Views.Settings;

using System.Collections.Immutable;
using System.Globalization;
using Core.Common.Settings;
using Core.Queries;
using Domain;
using MediatR;

internal sealed class SettingsViewModel : BasePageViewModel
{
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    private List<AccountLiteViewModel> availableAccounts = new();
    private IReadOnlyList<CurrencyViewModel> availableCurrencies = ImmutableList<CurrencyViewModel>.Empty;
    private AccountLiteViewModel? selectedAccount;
    private CurrencyViewModel selectedCurrency = null!;

    public SettingsViewModel(ISettingsFacade settingsFacade, IMediator mediator)
    {
        this.settingsFacade = settingsFacade;
        this.mediator = mediator;
    }

    public CurrencyViewModel SelectedCurrency
    {
        get => selectedCurrency;

        set
        {
            SetProperty(field: ref selectedCurrency, newValue: value);
            settingsFacade.DefaultCurrency = selectedCurrency.AlphaIsoCode;
        }
    }

    public IReadOnlyList<CurrencyViewModel> AvailableCurrencies
    {
        get => availableCurrencies;

        private set => SetProperty(field: ref availableCurrencies, newValue: value);
    }

    public AccountLiteViewModel? SelectedAccount
    {
        get => selectedAccount;

        set
        {
            SetProperty(field: ref selectedAccount, newValue: value);
            settingsFacade.DefaultAccount = selectedAccount?.Id ?? default;
        }
    }

    public List<AccountLiteViewModel> AvailableAccounts
    {
        get => availableAccounts;
        set => SetProperty(field: ref availableAccounts, newValue: value);
    }

    public async Task InitializeAsync()
    {
        var accounts = await mediator.Send(new GetAccountsQuery());
        AvailableAccounts = accounts.Select(a => new AccountLiteViewModel(Id: a.Id, Name: a.Name)).ToList();
        AvailableCurrencies = Currencies.GetAll().Select(c => new CurrencyViewModel(c.AlphaIsoCode)).OrderBy(c => c.AlphaIsoCode).ToList();
        var currencyToLoad = string.IsNullOrEmpty(settingsFacade.DefaultCurrency) ? RegionInfo.CurrentRegion.ISOCurrencySymbol : settingsFacade.DefaultCurrency;
        SelectedCurrency = AvailableCurrencies.FirstOrDefault(c => c.AlphaIsoCode == currencyToLoad) ?? AvailableCurrencies[0];
        SelectedAccount = AvailableAccounts.Find(x => x.Id == settingsFacade.DefaultAccount) ?? AvailableAccounts.FirstOrDefault();
    }
}
