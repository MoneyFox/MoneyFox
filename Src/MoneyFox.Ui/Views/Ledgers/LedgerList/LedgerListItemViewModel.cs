namespace MoneyFox.Ui.Views.Ledgers.LedgerList;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class LedgerListItemViewModel : ObservableObject
{
    private decimal currentBalance;
    private decimal endOfMonthBalance;

    private int id;
    private bool isExcluded;
    private string name = "";

    public required int Id
    {
        get => id;

        init => SetProperty(field: ref id, newValue: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public required decimal CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(field: ref currentBalance, newValue: value);
    }

    public required decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set => SetProperty(field: ref endOfMonthBalance, newValue: value);
    }

    public required bool IsExcluded
    {
        get => isExcluded;
        set => SetProperty(field: ref isExcluded, newValue: value);
    }
}
