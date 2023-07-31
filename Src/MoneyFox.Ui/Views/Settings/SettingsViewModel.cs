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

    private AccountLiteViewModel? selectedAccount = null;

    private IEnumerable<AccountLiteViewModel>? availableAccounts;

    public SettingsViewModel(ISettingsFacade settingsFacade, IMediator mediator)
    {
        this.settingsFacade = settingsFacade;
        this.mediator = mediator;
        AvailableCurrencies = Currencies.GetAll().Select(c => new CurrencyViewModel(c.AlphaIsoCode)).OrderBy(c => c.AlphaIsoCode).ToList();
        var currencyToLoad = string.IsNullOrEmpty(settingsFacade.DefaultCurrency) ? RegionInfo.CurrentRegion.ISOCurrencySymbol : settingsFacade.DefaultCurrency;
        SelectedCurrency = AvailableCurrencies.FirstOrDefault(c => c.AlphaIsoCode == currencyToLoad) ?? AvailableCurrencies[0];
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
        get => selectedAccount ??= AvailableAccounts?.FirstOrDefault(x => x.Name == (string.IsNullOrEmpty(settingsFacade.DefaultAccount) ? string.Empty : settingsFacade.DefaultAccount));

        set
        {
            SetProperty(field: ref selectedAccount, newValue: value);
            settingsFacade.DefaultAccount = selectedAccount != null ? selectedAccount.Name : string.Empty;
            OnPropertyChanged();
        }
    }

    public void LoadAccounts()
    {
        availableAccounts = mediator.Send(new GetAccountsQuery()).Result.Select(a => new AccountLiteViewModel(a.Name)).ToList();
    }

    public IEnumerable<AccountLiteViewModel>? AvailableAccounts
    {
        get
        {
            if(availableAccounts == null)
            {
                LoadAccounts();
            }
            return availableAccounts;
        }
    }
}
