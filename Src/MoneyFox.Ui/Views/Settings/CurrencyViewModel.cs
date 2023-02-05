namespace MoneyFox.Ui.Views.Settings;

public sealed record CurrencyViewModel(string AlphaIsoCode)
{
    public override string ToString()
    {
        return $"{AlphaIsoCode}";
    }
}
