using System.Collections.Generic;
using System.Linq;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPaymentRepository paymentRepository;

        public BalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository)
        {
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
        }

        private bool IsPaymentView { get; set; }

        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        /// <param name="isPaymentView">Indicates if the current view is a payment view or a generell overview.</param>
        public void UpdateBalance(bool isPaymentView = false)
        {
            IsPaymentView = isPaymentView;

            TotalBalance = GetTotalBalance();
            EndOfMonthBalance = GetEndOfMonthValue();
        }

        private double GetTotalBalance()
        {
            if (IsPaymentView)
            {
                return accountRepository.Selected.CurrentBalance;
            }

            return accountRepository.Data?.Sum(x => x.CurrentBalance) ?? 0;
        }

        private double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPayments = LoadUnclreadPayments();

            foreach (var payment in unclearedPayments)
            {
                switch (payment.Type)
                {
                    case (int) PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case (int) PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case (int) PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(Payment payment, double balance)
        {
            if (accountRepository.Selected == payment.ChargedAccount)
            {
                balance -= payment.Amount;
            }
            else
            {
                balance += payment.Amount;
            }
            return balance;
        }

        private IEnumerable<Payment> LoadUnclreadPayments()
        {
            var unclearedPayments = paymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth());

            return IsPaymentView
                ? unclearedPayments.Where(
                    x =>
                        x.ChargedAccountId == accountRepository.Selected.Id ||
                        x.TargetAccountId == accountRepository.Selected.Id).ToList()
                : unclearedPayments;
        }
    }
}