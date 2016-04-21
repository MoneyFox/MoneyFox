using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Interfaces.ViewModels;
using MoneyManager.Core.Helpers;
using PropertyChanged;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : ViewModelBase, IBalanceViewModel
    {
        protected readonly IAccountRepository AccountRepository;
        protected readonly IPaymentRepository PaymentRepository;

        public BalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository)
        {
            AccountRepository = accountRepository;
            PaymentRepository = paymentRepository;
        }

        /// <summary>
        ///     Balance of all relevant accounts at the end of the month.
        /// </summary>
        public double TotalBalance { get; set; }

        /// <summary>
        ///     Current Balance of all accounts.
        /// </summary>
        public double EndOfMonthBalance { get; set; }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a PaymentViewModel view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        public RelayCommand UpdateBalanceCommand => new RelayCommand(UpdateBalance);

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a PaymentViewModel view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        private void UpdateBalance()
        {
            TotalBalance = GetTotalBalance();
            EndOfMonthBalance = GetEndOfMonthValue();
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected virtual double GetTotalBalance()
        {
            return AccountRepository.Data?.Sum(x => x.CurrentBalance) ?? 0;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all PaymentViewModels to come till end of month.</returns>
        protected virtual double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPaymentViewModels = PaymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth());

            foreach (var paymentViewModel in unclearedPaymentViewModels)
            {
                //Transfer can be ignored since they don't change the summary.
                switch (paymentViewModel.Type)
                {
                    case PaymentType.Expense:
                        balance -= paymentViewModel.Amount;
                        break;

                    case PaymentType.Income:
                        balance += paymentViewModel.Amount;
                        break;
                }
            }

            return balance;
        }
    }
}