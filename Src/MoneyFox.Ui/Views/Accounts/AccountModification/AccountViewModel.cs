namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using CommunityToolkit.Mvvm.ComponentModel;

public class AccountViewModel : ObservableObject
{
    private DateTime created;
    private decimal currentBalance;
    private int id;
    private bool isExcluded;
    private DateTime? lastModified;
    private string name = "";
    private string? note;

    public int Id
    {
        get => id;
        init => SetProperty(field: ref id, newValue: value);
    }

    public string Name
    {
        get => name;

        set
        {
            SetProperty(field: ref name, newValue: value);
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public decimal CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(field: ref currentBalance, newValue: value);
    }

    public string? Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set => SetProperty(field: ref isExcluded, newValue: value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty(field: ref created, newValue: value);
    }

    public DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(field: ref lastModified, newValue: value);
    }

    public bool IsValid => string.IsNullOrEmpty(Name) is false;
}
