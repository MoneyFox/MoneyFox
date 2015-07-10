using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Manager
{
    public class RepositoryManager
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository<RecurringTransaction> _recurringTransactionRepository;
        private readonly IRepository<Category> _categoryRepository;

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
            _accountRepository.Data = null;
            _accountRepository.Selected = null;

            _transactionRepository.Data = null;
            _transactionRepository.Selected = null;

            _recurringTransactionRepository.Data = null;
            _recurringTransactionRepository.Selected = null;

            _categoryRepository.Data = null;
            _categoryRepository.Selected = null;
        }
    }
}