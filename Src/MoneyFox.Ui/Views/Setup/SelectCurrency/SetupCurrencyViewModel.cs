namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

using System.Globalization;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Domain;
using SetupAccounts;

public class SetupCurrencyViewModel : NavigableViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISettingsFacade settingsFacade;

    public SetupCurrencyViewModel(ISettingsFacade settingsFacade, INavigationService navigationService)
    {
        CurrencyViewModels = Currencies.GetAll().Select(c => new CurrencyViewModel(c.AlphaIsoCode)).OrderBy(c => c.AlphaIsoCode).ToList();
        SelectedCurrency = CurrencyViewModels.FirstOrDefault(c => c.AlphaIsoCode == RegionInfo.CurrentRegion.ISOCurrencySymbol) ?? CurrencyViewModels[0];
        this.settingsFacade = settingsFacade;
        this.navigationService = navigationService;
    }

    public IReadOnlyList<CurrencyViewModel> CurrencyViewModels { get; }

    public CurrencyViewModel SelectedCurrency { get; set; }

    public AsyncRelayCommand NextStepCommand => new(NextStep);

    public AsyncRelayCommand BackCommand => new(() => navigationService.GoBack());

    private async Task NextStep()
    {
        settingsFacade.DefaultCurrency = SelectedCurrency.AlphaIsoCode;
        await navigationService.GoTo<SetupAccountsViewModel>();
    }
}
