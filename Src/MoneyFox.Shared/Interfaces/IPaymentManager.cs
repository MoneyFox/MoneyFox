using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IPaymentManager
    {
        bool SavePayment(Payment payment);

        void DeleteAssociatedPaymentsFromDatabase(Account account);

        void RemoveRecurringForPayment(Payment paymentToChange);

        Task<bool> CheckForRecurringPayment(Payment payment);

        IEnumerable<Payment> LoadRecurringPaymentList(Func<Payment, bool> filter = null);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPayment recurringPayment);

        bool AddPaymentAmount(Payment payment);

        bool RemovePaymentAmount(Payment payment);

        bool RemovePaymentAmount(Payment payment, Account account);
    }
}