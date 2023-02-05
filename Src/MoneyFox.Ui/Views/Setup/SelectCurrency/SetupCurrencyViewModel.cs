namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

using System.Globalization;
using Common;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Helpers;
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
