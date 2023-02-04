namespace MoneyFox.Ui.Views.SetupAssistant.CurrencyIntroduction;

public sealed record CurrencyViewModel(string Currency)
{
    public override string ToString()
    {
        return $"{Currency}";
    }
}
