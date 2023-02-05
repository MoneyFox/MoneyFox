namespace MoneyFox.Ui.Views.Setup.CurrencyIntroduction;

public sealed record CurrencyViewModel(string AlphaIsoCode)
{
    public override string ToString()
    {
        return $"{AlphaIsoCode}";
    }
}
