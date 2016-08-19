using System;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager
{
    /// <summary>
    ///     This helper can be used to reinstantiate all Repositories, for example when you
    ///     download a new database backup and replace the current one.
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly ICategoryRepository categoryRepository;

        private readonly IPaymentManager paymentManager;

        public RepositoryManager(IPaymentManager paymentManager, 
            IAccountRepository accountRepository,
            IPaymentRepository paymentRepository, 
            ICategoryRepository categoryRepository)
        {
            this.paymentManager = paymentManager;
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        ///     This will reload all Data for the repositories and set the Selected Property to null.
        ///     After this it checks if there are payments to cleare and if so will clear them.
        /// </summary>
        public void ReloadData()
        {
            //Load Data
            accountRepository.Load();

            paymentRepository.Load();
            paymentRepository.Selected = null;

            categoryRepository.Load();

            //check if there are payments to clear
            paymentManager.ClearPayments();
        }
    }
}