using System.Collections.Generic;
using System.Linq;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly ITransactionRepository transactionRepository;

        public BalanceViewModel(IRepository<Account> accountRepository,
            ITransactionRepository transactionRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
        }

        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }
        public bool IsTransactionView { private get; set; }

        /// <summary>
        ///     Refreshes the balances
        /// </summary>
        public void UpdateBalance()
        {
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