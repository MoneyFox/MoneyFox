namespace MoneyFox.Ui.Views.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Core.Common.Settings;
using Domain;
using MediatR;
using MoneyFox.Core.Queries;
using MoneyFox.Ui.Views.Accounts.AccountModification;

internal sealed class SettingsViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    private readonly IMediator mediator;

    private readonly IMapper mapper;

    private CurrencyViewModel selectedCurrency = null!;

    private AccountViewModel? selectedAccount = null;

    
    public SettingsViewModel(ISettingsFacade settingsFacade, IMediator mediator, IMapper mapper)
    {
        this.settingsFacade = settingsFacade;
        this.mediator = mediator;
        this.mapper = mapper;
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

    public AccountViewModel? SelectedAccount
    {
        get => selectedAccount ??= AvailableAccounts?.FirstOrDefault(x => x.Name == (string.IsNullOrEmpty(settingsFacade.DefaultAccount) ? string.Empty : settingsFacade.DefaultAccount));

        set
        {
            SetProperty(field: ref selectedAccount, newValue: value);
            settingsFacade.DefaultAccount = selectedAccount != null ? selectedAccount.Name : string.Empty;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<AccountViewModel>? AvailableAccounts => mapper.Map<ObservableCollection<AccountViewModel>>(mediator.Send(new GetAccountsQuery()).Result);
}
