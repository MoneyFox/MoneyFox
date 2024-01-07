namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.AccountAggregate;

public sealed class AccountViewModel : ObservableObject, IEquatable<AccountViewModel>
{
    private DateTime created;
    private decimal currentBalance;
    private decimal endOfMonthBalance;

    private int id;
    private bool isExcluded;
    private bool isOverdrawn;
    private DateTime? lastModified;
    private string name = "";
    private string note = "";

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public decimal CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(field: ref currentBalance, newValue: value);
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set => SetProperty(field: ref endOfMonthBalance, newValue: value);
    }

    public string Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public bool IsOverdrawn
    {
        get => isOverdrawn;
        set => SetProperty(field: ref isOverdrawn, newValue: value);
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

    public bool Equals(AccountViewModel? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
