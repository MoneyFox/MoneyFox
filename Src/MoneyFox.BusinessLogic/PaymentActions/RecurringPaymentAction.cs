using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    public interface IRecurringPaymentAction
    {
        Task CreatePaymentsUpToRecur();
    }

    public class RecurringPaymentAction : IRecurringPaymentAction
    {
        private readonly IRecurringPaymentDbAccess recurringPaymentDbAccess;

        public RecurringPaymentAction(IRecurringPaymentDbAccess recurringPaymentDbAccess)
        {
            this.recurringPaymentDbAccess = recurringPaymentDbAccess;
        }

        public async Task CreatePaymentsUpToRecur()
        {
            var recurringPayments = await recurringPaymentDbAccess.GetRecurringPayments();

            await recurringPaymentDbAccess.SaveNewPayments(recurringPayments.Where(x => RecurringPaymentHelper
                    .CheckIfRepeatable(x.RelatedPayments
                        .OrderByDescending(d => d.Date)
                        .First()))
                .Select(x => new Payment(
                    DateTime.Now,
                    x.Amount,
                    x.Type,
                    x.ChargedAccount,
                    x.TargetAccount,
                    x.Category,
                    x.Note))
                .ToList());
        }
    }
}
