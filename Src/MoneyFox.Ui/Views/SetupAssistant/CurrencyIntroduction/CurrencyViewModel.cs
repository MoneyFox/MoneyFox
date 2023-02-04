namespace MoneyFox.Ui.Views.SetupAssistant.CurrencyIntroduction;

public sealed record CurrencyViewModel(string AlphaIsoCode)
{
    public override string ToString()
    {
        return $"{AlphaIsoCode}";
    }
}
