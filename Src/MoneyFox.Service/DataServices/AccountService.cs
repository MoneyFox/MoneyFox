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
        private readonly IAccountRepository accountRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task DeleteAccount(Account account)
        {
            accountRepository.Delete(account.Data);
            await unitOfWork.Commit();
        }
    }
}
