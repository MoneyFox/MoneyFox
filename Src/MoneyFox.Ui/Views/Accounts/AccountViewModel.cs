namespace MoneyFox.Ui.Views.Accounts;

using AutoMapper;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.AccountAggregate;

public sealed class AccountViewModel : ObservableViewModelBase, IHaveCustomMapping, IEquatable<AccountViewModel>
{
    private DateTime created;
    private decimal currentBalance;
    private decimal endOfMonthBalance;

    private int id;
    private bool isExcluded;
    private bool isOverdrawn;
    private DateTime lastModified;
    private string name = "";
    private string note = "";

    public int Id
    {
        get => id;

        set => SetProperty(property: ref id, value: value);
    }

    public string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public decimal CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(property: ref currentBalance, value: value);
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set => SetProperty(property: ref endOfMonthBalance, value: value);
    }

    public string Note
    {
        get => note;
        set => SetProperty(property: ref note, value: value);
    }

    public bool IsOverdrawn
    {
        get => isOverdrawn;
        set => SetProperty(property: ref isOverdrawn, value: value);
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set => SetProperty(property: ref isExcluded, value: value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty(property: ref created, value: value);
    }

    public DateTime LastModified
    {
        get => lastModified;
        set => SetProperty(property: ref lastModified, value: value);
    }

    public bool Equals(AccountViewModel? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Account, AccountViewModel>();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
