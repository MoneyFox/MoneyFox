using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.Pocos;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Service.QueryExtensions;

namespace MoneyFox.Service.DataServices
{
    /// <summary>
    ///     Offers service methods to access and modify account data.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        ///     Returns a list with all accounts.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Account>> GetAllAccounts();

        /// <summary>
        ///     Returns the number of existing Accounts.
        /// </summary>
        Task<int> GetAccountCount();

        /// <summary>
        ///     Returns a list with all not excluded Accounts.
        /// </summary>
        /// <returns>List with all not excluded Accounts.</returns>
        Task<IEnumerable<Account>> GetNotExcludedAccounts();

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
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await accountRepository.GetAll().SelectAccounts().ToListAsync();
        }

        /// <inheritdoc />
        public Task<int> GetAccountCount()
        {
            return accountRepository.GetAll().CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetNotExcludedAccounts()
        {
            return await accountRepository
                .GetAll()
                .AreNotExcluded()
                .SelectAccounts()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAccount(Account account)
        {
            accountRepository.Delete(account.Data);
            await unitOfWork.Commit();
        }
    }
}
