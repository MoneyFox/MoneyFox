namespace MoneyFox.Win.ViewModels.Accounts;

using CommunityToolkit.Mvvm.ComponentModel;
using Core._Pending_.Common.Interfaces.Mapping;
using Core.Aggregates;
using System;

public sealed class AccountViewModel : ObservableObject, IMapFrom<Account>, IEquatable<AccountViewModel>
{
    private const decimal DECIMAL_DELTA = 0.01m;

    private int id;
    private string name = "";
    private decimal currentBalance;
    private decimal endOfMonthBalance;
    private string note = "";
    private bool isOverdrawn;
    private bool isExcluded;
    private DateTime created;
    private DateTime lastModified;

    public int Id
    {
        get => id;
        set
        {
            if(id == value)
            {
                return;
            }

            id = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => name;
        set
        {
            if(name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    public decimal CurrentBalance
    {
        get => currentBalance;
        set
        {
            if(Math.Abs(currentBalance - value) < DECIMAL_DELTA)
            {
                return;
            }

            currentBalance = value;
            OnPropertyChanged();
        }
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set
        {
            endOfMonthBalance = value;
            OnPropertyChanged();
        }
    }

    public string Note
    {
        get => note;
        set
        {
            if(note == value)
            {
                return;
            }

            note = value;
            OnPropertyChanged();
        }
    }

    public bool IsOverdrawn
    {
        get => isOverdrawn;
        set
        {
            if(isOverdrawn == value)
            {
                return;
            }

            isOverdrawn = value;
            OnPropertyChanged();
        }
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set
        {
            if(isExcluded == value)
            {
                return;
            }

            isExcluded = value;
            OnPropertyChanged();
        }
    }

    public DateTime Created
    {
        get => created;
        set
        {
            if(created == value)
            {
                return;
            }

            created = value;
            OnPropertyChanged();
        }
    }

    public DateTime LastModified
    {
        get => lastModified;
        set
        {
            if(lastModified == value)
            {
                return;
            }

            lastModified = value;
            OnPropertyChanged();
        }
    }

    public bool Equals(AccountViewModel other)
    {
        if(other == null)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if(obj is AccountViewModel vm)
        {
            return Equals(vm);
        }

        return false;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(Name);
        hash.Add(CurrentBalance);
        hash.Add(EndOfMonthBalance);
        hash.Add(Note);
        hash.Add(IsOverdrawn);
        hash.Add(IsExcluded);
        hash.Add(Created);
        hash.Add(LastModified);
        return hash.ToHashCode();
    }
}