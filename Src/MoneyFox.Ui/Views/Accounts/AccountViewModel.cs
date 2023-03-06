namespace MoneyFox.Ui.Views.Accounts;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.AccountAggregate;

public sealed class AccountViewModel : ObservableObject, IHaveCustomMapping, IEquatable<AccountViewModel>
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

        set => SetProperty(ref id, value);
    }

    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public decimal CurrentBalance
    {
        get => currentBalance;
        set => SetProperty(ref currentBalance, value);
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set => SetProperty(ref endOfMonthBalance, value);
    }

    public string Note
    {
        get => note;
        set => SetProperty(ref note, value);
    }

    public bool IsOverdrawn
    {
        get => isOverdrawn;
        set => SetProperty(ref isOverdrawn, value);
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set => SetProperty(ref isExcluded, value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty(ref created, value);
    }

    public DateTime LastModified
    {
        get => lastModified;
        set => SetProperty(ref lastModified, value);
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
