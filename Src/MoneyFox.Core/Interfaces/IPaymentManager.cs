using System.Threading.Tasks;
using MoneyFox.Core.DatabaseModels;
using MoneyFox.Core.ViewModels.Models;

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