namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

public sealed record CurrencyViewModel(string AlphaIsoCode, string CultureDisplayName)
{
    public override string ToString()
    {
        return $"{CultureDisplayName} ({AlphaIsoCode})";
    }
}
