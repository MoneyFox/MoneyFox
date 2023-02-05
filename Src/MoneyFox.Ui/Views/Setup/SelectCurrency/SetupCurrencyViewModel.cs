namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

using System.Collections.Immutable;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Facades;
using Domain;

public class SetupCurrencyViewModel : BasePageViewModel
{
    private readonly ISettingsFacade settingsFacade;

    public SetupCurrencyViewModel(ISettingsFacade settingsFacade)
    {
        CurrencyViewModels = Currencies.GetAll().Select(c => new CurrencyViewModel(c.AlphaIsoCode)).ToImmutableList();
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
}
