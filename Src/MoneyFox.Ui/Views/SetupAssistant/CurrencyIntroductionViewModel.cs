namespace MoneyFox.Ui.Views.SetupAssistant;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Domain;

public class CurrencyIntroductionViewModel : BasePageViewModel
{
    public CurrencyIntroductionViewModel()
    {
        CurrencyViewModels = Currencies.GetAll()
                                       .Select(c => new CurrencyViewModel { IsoNumericCode = c.NumericIsoCode, Country = new CultureInfo(c.NumericIsoCode).DisplayName, Currency = c.AlphaIsoCode })
                                       .ToImmutableList();

        SelectedCurrency = CurrencyViewModels.FirstOrDefault(c => c.IsoNumericCode == CultureInfo.CurrentCulture.LCID) ?? CurrencyViewModels.First();
    }

    public IReadOnlyList<CurrencyViewModel> CurrencyViewModels { get; }

    public CurrencyViewModel SelectedCurrency { get; set; }

    public AsyncRelayCommand NextStepCommand => new(async () => await Shell.Current.GoToAsync(Routes.CategoryIntroductionRoute));

    public AsyncRelayCommand BackCommand => new(Shell.Current.Navigation.PopAsync);
}

public sealed class CurrencyViewModel
{
    public required int IsoNumericCode { get; init; }
    public required string Country { get; init; }
    public required string Currency { get; init; }

    public override string ToString()
    {
        return $"{Country} ({Currency})";
    }
}
