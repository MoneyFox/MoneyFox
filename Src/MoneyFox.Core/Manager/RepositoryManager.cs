using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;

namespace MoneyManager.Core.Manager
{
    /// <summary>
    ///     This helper can be used to reinstantiate all Repositories, for example when you
    ///     download a new database backup and replace the current one.
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        public RepositoryManager(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository,
            IPaymentManager paymentManager)
        {
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
            this.paymentManager = paymentManager;
        }

        /// <summary>
        ///     This will reload all Data for the repositories and set the Selected Property to null.
        ///     After this it checks if there are payments to cleare and if so will clear them.
        /// </summary>
        public void ReloadData()
        {
            //Load Data
            accountRepository.Load();
            accountRepository.Selected = null;

            paymentRepository.Load();
            paymentRepository.Selected = null;

            categoryRepository.Load();
            categoryRepository.Selected = null;

            //check if there are payments to clear
            paymentManager.ClearPayments();
        }
    }
}