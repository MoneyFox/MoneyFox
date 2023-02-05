namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

public sealed record CurrencyViewModel(string AlphaIsoCode, string RegionDisplayName)
{
    public override string ToString()
    {
        return $"{RegionDisplayName} ({AlphaIsoCode})";
    }
}
