namespace MoneyFox.Ui.ViewModels.Accounts;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.Common.Interfaces.Mapping;

public sealed class AccountViewModel : ObservableObject, IHaveCustomMapping, IEquatable<AccountViewModel>
{
    private const decimal DECIMAL_DELTA = 0.01m;
    private DateTime creationTime;
    private decimal currentBalance;
    private decimal endOfMonthBalance;

    private int id;
    private bool isExcluded;
    private bool isOverdrawn;
    private DateTime modificationDate;
    private string name = "";
    private string note = "";

    /// <summary>
    ///     Account Id
    /// </summary>
    public int Id
    {
        get => id;

        set
        {
            if (id == value)
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
            if (name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Current Balance
    /// </summary>
    public decimal CurrentBalance
    {
        get => currentBalance;

        set
        {
            if (Math.Abs(currentBalance - value) < DECIMAL_DELTA)
            {
                return;
            }

            currentBalance = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Balance End of Month
    /// </summary>
    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;

        set
        {
            endOfMonthBalance = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Note
    /// </summary>
    public string Note
    {
        get => note;

        set
        {
            if (note == value)
            {
                return;
            }

            note = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicator if the account currently is overdrawn.
    /// </summary>
    public bool IsOverdrawn
    {
        get => isOverdrawn;

        set
        {
            if (isOverdrawn == value)
            {
                return;
            }

            isOverdrawn = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicator if the account is excluded from the balance calculation.
    /// </summary>
    public bool IsExcluded
    {
        get => isExcluded;

        set
        {
            if (isExcluded == value)
            {
                return;
            }

            isExcluded = value;
            OnPropertyChanged();
        }
    }

    public DateTime CreationTime
    {
        get => creationTime;

        set
        {
            if (creationTime == value)
            {
                return;
            }

            creationTime = value;
            OnPropertyChanged();
        }
    }

    public DateTime ModificationDate
    {
        get => modificationDate;

        set
        {
            if (modificationDate == value)
            {
                return;
            }

            modificationDate = value;
            OnPropertyChanged();
        }
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Account, AccountViewModel>();
    }

    public bool Equals(AccountViewModel other)
    {
        if (other == null)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
