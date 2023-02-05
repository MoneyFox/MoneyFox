namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Domain;

public class SetupCurrencyViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public SetupCurrencyViewModel(ISettingsFacade settingsFacade)
    {
        CurrencyViewModels = GetCurrencyViewModels();
        SelectedCurrency = CurrencyViewModels.FirstOrDefault(c => c.AlphaIsoCode == RegionInfo.CurrentRegion.ISOCurrencySymbol) ?? CurrencyViewModels.First();
        this.settingsFacade = settingsFacade;
    }

    public IReadOnlyList<CurrencyViewModel> CurrencyViewModels { get; }

    public CurrencyViewModel SelectedCurrency { get; set; }

    public AsyncRelayCommand NextStepCommand => new(NextStep);

    public AsyncRelayCommand BackCommand => new(Shell.Current.Navigation.PopAsync);

    private async Task NextStep()
    {
        settingsFacade.DefaultCurrency = SelectedCurrency.AlphaIsoCode;
        await Shell.Current.GoToAsync(Routes.SetupAccountsRoute);
    }

    private static List<CurrencyViewModel> GetCurrencyViewModels()
    {
        var isoCurrenciesToACultureMap = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new { c, new RegionInfo(c.Name).ISOCurrencySymbol })
                .GroupBy(x => x.ISOCurrencySymbol)
                .ToDictionary(keySelector: g => g.Key, elementSelector: g => g.First().c, comparer: StringComparer.OrdinalIgnoreCase);

        var currencyVmList = new List<CurrencyViewModel>();
        foreach (var CurrencyIsoCode in Currencies.GetAll().Select(c => c.AlphaIsoCode))
        {
            if (isoCurrenciesToACultureMap.TryGetValue(key: CurrencyIsoCode, value: out var culture))
            {
                currencyVmList.Add(new CurrencyViewModel(CurrencyIsoCode, new RegionInfo(culture.Name).DisplayName));
            }
        }

        return currencyVmList;
    }
}
