namespace MoneyFox.Ui.Views.Settings;

using System.Globalization;
using Core.Common.Helpers;
using Core.Common.Settings;
using Domain;

internal sealed class SettingsViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    private CurrencyViewModel selectedCurrency = null!;

    public SettingsViewModel(ISettingsFacade settingsFacade)
    {
        this.settingsFacade = settingsFacade;
        AvailableCurrencies = GetCurrencyViewModels();
        SelectedCurrency = AvailableCurrencies.FirstOrDefault(c => c.AlphaIsoCode == RegionInfo.CurrentRegion.ISOCurrencySymbol) ?? AvailableCurrencies.First();
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

    private static List<CurrencyViewModel> GetCurrencyViewModels()
    {
        var currencyVmList = new List<CurrencyViewModel>();
        foreach (var currencyIsoCode in Currencies.GetAll().Select(c => c.AlphaIsoCode))
        {
            if (CurrencyHelper.IsoCurrenciesToACultureMap.TryGetValue(key: currencyIsoCode, value: out var culture))
            {
                currencyVmList.Add(new(AlphaIsoCode: currencyIsoCode, RegionDisplayName: new RegionInfo(culture.Name).DisplayName));
            }
        }

        return currencyVmList;
    }
}
