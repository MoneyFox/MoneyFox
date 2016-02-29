using System.Threading.Tasks;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IPaymentManager
    {
        void DeleteAssociatedPaymentsFromDatabase(Account account);

        Task<bool> CheckForRecurringPayment(Payment payment);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPayment recurringPayment);
    }
}