namespace MoneyFox.Ui.Controls.AccountPicker;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain;

public class AccountPickerViewModel : ObservableObject
{
    private int id;
    private string name = "";
    private Money money = Money.Zero("USD");

    public required int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public required Money CurrentBalance
    {
        get => money;
        set => SetProperty(field: ref money, newValue: value);
    }
}
