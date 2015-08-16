using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Manager
{
    public class RepositoryManager
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<RecurringTransaction> _recurringTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;

        public RepositoryManager(IRepository<Account> accountRepository,
            ITransactionRepository transactionRepository,
            IRepository<RecurringTransaction> recurringTransactionRepository,
            IRepository<Category> categoryRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _recurringTransactionRepository = recurringTransactionRepository;
            _categoryRepository = categoryRepository;
        }

        public void ReloadData()
        {
            _accountRepository.Load();
            _accountRepository.Selected = null;

            _transactionRepository.Load();
            _transactionRepository.Selected = null;

            _recurringTransactionRepository.Load();
            _recurringTransactionRepository.Selected = null;

            _categoryRepository.Load();
            _categoryRepository.Selected = null;
        }
    }
}