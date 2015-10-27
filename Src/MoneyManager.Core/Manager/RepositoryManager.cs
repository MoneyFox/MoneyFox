using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class RepositoryManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly TransactionManager transactionManager;

        public RepositoryManager(IRepository<Account> accountRepository,
            ITransactionRepository transactionRepository,
            IRepository<Category> categoryRepository,
            TransactionManager transactionManager)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionManager = transactionManager;
        }

        public void ReloadData()
        {
            //Load Data
            accountRepository.Load();
            accountRepository.Selected = null;

            transactionRepository.Load();
            transactionRepository.Selected = null;

            categoryRepository.Load();
            categoryRepository.Selected = null;

            //check if there are transactions to clear
            transactionManager.ClearTransactions();
        }
    }
}