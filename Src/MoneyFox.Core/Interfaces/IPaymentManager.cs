using System.Threading.Tasks;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IPaymentManager
    {
        void DeleteAssociatedPaymentsFromDatabase(Account account);

        Task<bool> CheckForRecurringPayment(PaymentViewModel payment);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPaymentViewModel recurringPayment);
    }
}