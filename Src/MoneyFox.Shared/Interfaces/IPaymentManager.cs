using System.Threading.Tasks;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces {
    public interface IPaymentManager {
        void DeleteAssociatedPaymentsFromDatabase(Account account);

        Task<bool> CheckForRecurringPayment(Payment payment);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPayment recurringPayment);
    }
}