using System.Linq;
using MoneyFox.Core.Helpers;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Manager
{
    public class RecurringPaymentViewModelManager : IRecurringPaymentViewModelManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPaymentRepository paymentRepository;

        public RecurringPaymentViewModelManager(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        ///     Checks if one of the recurring PaymentViewModel has to be repeated
        /// </summary>
        public void CheckRecurringPaymentViewModels()
        {
            var paymentViewModelList = paymentRepository.LoadRecurringList();

            foreach (var paymentViewModel in paymentViewModelList.Where(x => x.ChargedAccount != null))
            {
                var relatedPaymentViewModel = GetLastOccurence(paymentViewModel);

                if (RecurringPaymentViewModelHelper.CheckIfRepeatable(paymentViewModel.RecurringPayment, relatedPaymentViewModel))
                {
                    var newPaymentViewModel = RecurringPaymentViewModelHelper.GetPaymentViewModelFromRecurring(paymentViewModel.RecurringPayment);

                    paymentRepository.Save(newPaymentViewModel);
                    accountRepository.AddPaymentAmount(newPaymentViewModel);
                }
            }
        }

        private PaymentViewModel GetLastOccurence(PaymentViewModel paymentVm)
        {
            var transcationList = paymentRepository.Data
                .Where(x => x.RecurringPayment == paymentVm.RecurringPayment)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.LastOrDefault();
        }
    }
}