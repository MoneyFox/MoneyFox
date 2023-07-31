namespace MoneyFox.Ui.Views.Settings;
public sealed record AccountLiteViewModel(string Name)
{
    public override string ToString()
    {
        return $"{Name}";
    }
}
