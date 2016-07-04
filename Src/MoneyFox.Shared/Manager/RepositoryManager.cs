using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Manager {
    /// <summary>
    ///     This helper can be used to reinstantiate all Repositories, for example when you
    ///     download a new database backup and replace the current one.
    /// </summary>
    public class RepositoryManager : IRepositoryManager {
        private readonly UnitOfWork unitOfWork;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        public RepositoryManager(UnitOfWork unitOfWork,
            IPaymentRepository paymentRepository,
            IPaymentManager paymentManager) {

            this.unitOfWork = unitOfWork;

            this.paymentRepository = paymentRepository;
            this.paymentManager = paymentManager;
        }

        /// <summary>
        ///     This will reload all Data for the repositories and set the Selected Property to null.
        ///     After this it checks if there are payments to cleare and if so will clear them.
        /// </summary>
        public void ReloadData() {
            //Load Data
            unitOfWork.AccountRepository.Load();

            paymentRepository.Load();
            paymentRepository.Selected = null;

            unitOfWork.CategoryRepository.Load();

            //check if there are payments to clear
            paymentManager.ClearPayments();
        }
    }
}