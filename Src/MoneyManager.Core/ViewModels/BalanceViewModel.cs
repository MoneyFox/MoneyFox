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
        private readonly ITransactionRepository transactionRepository;

        public BalanceViewModel(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;

            UpdateBalance();
        }

        private bool IsTransactionView { get; set; }

        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a transactionview or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        /// <param name="isTransactionView">Indicates if the current view is a transactionView or a generell overview.</param>
        public void UpdateBalance(bool isTransactionView = false)
        {
            IsTransactionView = isTransactionView;

            TotalBalance = GetTotalBalance();
            EndOfMonthBalance = GetEndOfMonthValue();
        }

        private double GetTotalBalance()
        {
            if (IsTransactionView)
            {
                return accountRepository.Selected.CurrentBalance;
            }

            return accountRepository.Data?.Sum(x => x.CurrentBalance) ?? 0;
        }

        private double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedTransactions = LoadUnclreadTransactions();

            foreach (var transaction in unclearedTransactions)
            {
                switch (transaction.Type)
                {
                    case (int) TransactionType.Spending:
                        balance -= transaction.Amount;
                        break;

                    case (int) TransactionType.Income:
                        balance += transaction.Amount;
                        break;

                    case (int) TransactionType.Transfer:
                        balance = HandleTransferAmount(transaction, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(FinancialTransaction transaction, double balance)
        {
            if (accountRepository.Selected == transaction.ChargedAccount)
            {
                balance -= transaction.Amount;
            }
            else
            {
                balance += transaction.Amount;
            }
            return balance;
        }

        private IEnumerable<FinancialTransaction> LoadUnclreadTransactions()
        {
            var unclearedTransactions =
                transactionRepository.GetUnclearedTransactions(Utilities.GetEndOfMonth());

            return IsTransactionView
                ? unclearedTransactions.Where(
                    x =>
                        x.ChargedAccountId == accountRepository.Selected.Id ||
                        x.TargetAccountId == accountRepository.Selected.Id).ToList()
                : unclearedTransactions;
        }
    }
}