using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class RepositoryManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly ITransactionRepository transactionRepository;

        public RepositoryManager(IRepository<Account> accountRepository,
            ITransactionRepository transactionRepository,
            IRepository<Category> categoryRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
        }

        public void ReloadData()
        {
            accountRepository.Load();
            accountRepository.Selected = null;

            transactionRepository.Load();
            transactionRepository.Selected = null;

            categoryRepository.Load();
            categoryRepository.Selected = null;
        }
    }
}