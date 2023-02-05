namespace MoneyFox.Ui.Views.Settings;

public sealed record CurrencyViewModel(string AlphaIsoCode, string RegionDisplayName)
{
    public override string ToString()
    {
        return $"{RegionDisplayName} ({AlphaIsoCode})";
    }
}
