using System.Threading.Tasks;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service.DataServices
{
    /// <summary>
    ///     Offers service methods to access and modify account data.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        ///     Deletes the passed account and all associated payments.
        /// </summary>
        Task DeleteAccount(Account account);
    }

    /// <summary>
    ///     Offers service methods to access and modify account data.
    /// </summary>
    public class AccountService : IAccountService
    {
        private IAccountRepository accountRepository;
        private IPaymentService paymentService;
        private IUnitOfWork unitOfWork;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }

        public Task DeleteAccount(Account account)
        {
            paymentService.DeletePayments(account.Data.Payments);
            accountRepository.Delete(account.Data);
            unitOfWork.Commit()
        }
    }
}
