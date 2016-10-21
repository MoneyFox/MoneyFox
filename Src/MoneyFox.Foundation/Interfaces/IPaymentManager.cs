using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IPaymentManager
    {
        bool SavePayment(PaymentViewModel payment);

        void DeleteAssociatedPaymentsFromDatabase(AccountViewModel accountViewModel);

        void RemoveRecurringForPayment(PaymentViewModel paymentToChange);

        Task<bool> CheckRecurrenceOfPayment(PaymentViewModel payment);

        IEnumerable<PaymentViewModel> LoadRecurringPaymentList(Func<PaymentViewModel, bool> filter = null);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPaymentViewModel recurringPayment);

        bool AddPaymentAmount(PaymentViewModel payment);

        bool RemovePaymentAmount(PaymentViewModel payment);

        bool RemovePaymentAmount(PaymentViewModel payment, AccountViewModel accountViewModel);
    }
}