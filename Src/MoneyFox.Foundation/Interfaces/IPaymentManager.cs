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

        IEnumerable<PaymentViewModel> LoadRecurringPaymentList(Func<PaymentViewModel, bool> filter = null);

        void ClearPayments();

        bool AddPaymentAmount(PaymentViewModel payment);

        bool RemovePaymentAmount(PaymentViewModel payment);

        bool RemovePaymentAmount(PaymentViewModel payment, AccountViewModel accountViewModel);

        Task<bool> DeletePayment(PaymentViewModel payment);
    }
}