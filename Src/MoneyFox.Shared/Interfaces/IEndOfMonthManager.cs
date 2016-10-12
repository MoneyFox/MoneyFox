using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IEndOfMonthManager
    {
        void AssignToAccounts();

        void DeterminEndThroughAccounts(Account argAccount);
    }
}