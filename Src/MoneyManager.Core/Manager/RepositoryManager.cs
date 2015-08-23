using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Manager
{
    public class RepositoryManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<RecurringTransaction> recurringTransactionRepository;
        private readonly ITransactionRepository transactionRepository;

        public RepositoryManager(IRepository<Account> accountRepository,
            ITransactionRepository transactionRepository,
            IRepository<RecurringTransaction> recurringTransactionRepository,
            IRepository<Category> categoryRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.recurringTransactionRepository = recurringTransactionRepository;
            this.categoryRepository = categoryRepository;
        }

        public void ReloadData()
        {
            accountRepository.Load();
            accountRepository.Selected = null;

            transactionRepository.Load();
            transactionRepository.Selected = null;

            recurringTransactionRepository.Load();
            recurringTransactionRepository.Selected = null;

            categoryRepository.Load();
            categoryRepository.Selected = null;
        }
    }
}