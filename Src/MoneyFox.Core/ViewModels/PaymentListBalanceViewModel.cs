using System.Collections.Generic;
using System.Linq;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.ViewModels.Models;
using MoneyManager.Core.Helpers;
using MoneyManager.Core.ViewModels;

namespace MoneyFox.Core.ViewModels
{
    /// <summary>
    ///     This ViewModel is for the usage in the PaymentViewModellist when a concret account is selected
    /// </summary>
    public class PaymentViewModelListBalanceViewModel : BalanceViewModel
    {
        public PaymentViewModelListBalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository)
            : base(accountRepository, paymentRepository)
        {
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override double GetTotalBalance()
        {
            return AccountRepository.Selected.CurrentBalance;
        }

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.
        ///     This includes all PaymentViewModels coming until the end of month.
        /// </summary>
        /// <returns>Balance of the selected accont including all PaymentViewModels to come till end of month.</returns>
        protected override double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPaymentViewModels = LoadUnclreadPaymentViewModels();

            foreach (var paymentViewModel in unclearedPaymentViewModels)
            {
                switch (paymentViewModel.Type)
                {
                    case PaymentType.Expense:
                        balance -= paymentViewModel.Amount;
                        break;

                    case PaymentType.Income:
                        balance += paymentViewModel.Amount;
                        break;

                    case PaymentType.Transfer:
                        balance = HandleTransferAmount(paymentViewModel, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(PaymentViewModel PaymentViewModel, double balance)
        {
            if (AccountRepository.Selected == PaymentViewModel.ChargedAccount)
            {
                balance -= PaymentViewModel.Amount;
            }
            else
            {
                balance += PaymentViewModel.Amount;
            }
            return balance;
        }

        private IEnumerable<PaymentViewModel> LoadUnclreadPaymentViewModels()
        {
            return PaymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth())
                .Where(x => x.ChargedAccount.Id == AccountRepository.Selected.Id
                            || x.TargetAccount.Id == AccountRepository.Selected.Id)
                .ToList();
        }
    }
}