namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

public sealed record CurrencyViewModel(string AlphaIsoCode)
{
    public override string ToString()
    {
        return $"{AlphaIsoCode}";
    }
}
