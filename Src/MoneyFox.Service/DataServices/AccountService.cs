using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.DataAccess;
using MoneyFox.Service.Pocos;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Repositories;
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
        /// <returns>Returns a IEnumerable with all accounts.</returns>
        Task<IEnumerable<Account>> GetAllAccounts();

        /// <summary>
        ///     Returns a Account searched by ID.
        /// </summary>
        /// <param name="id">Id to select the account for.</param>
        /// <returns>The selected Account</returns>
        Task<Account> GetById(int id);

        /// <summary>
        ///     Returns the number of existing Accounts.
        /// </summary>
        /// <returns>Count of all existing accounts</returns>
        Task<int> GetAccountCount();

        /// <summary>
        ///     Checks if the name is already taken by another account.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>if account name is already taken.</returns>
        Task<bool> CheckIfNameAlreadyTaken(string name);

        /// <summary>
        ///     Returns a list with all not excluded Accounts.
        /// </summary>
        /// <returns>List with all not excluded Accounts.</returns>
        Task<IEnumerable<Account>> GetNotExcludedAccounts();
        
        /// <summary>
        ///     Returns a list with all excluded Accounts.
        /// </summary>
        /// <returns>List with all  excluded Accounts.</returns>
        Task<IEnumerable<Account>> GetExcludedAccounts();

        /// <summary>
        ///     Save the passed account.
        /// </summary>
        Task SaveAccount(Account account);

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
            var list = await accountRepository
                .GetAll()
                .OrderByName()
                .ToListAsync();

            return list.Select(x => new Account(x));
        }

        /// <inheritdoc />
        public async Task<Account> GetById(int id)
        {
            return new Account(await accountRepository.GetById(id));
        }

        /// <inheritdoc />
        public Task<int> GetAccountCount()
        {
            return accountRepository.GetAll().CountAsync();
        }

        /// <inheritdoc />
        public Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            return accountRepository.GetAll()
                                    .NameEquals(name)
                                    .AnyAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetNotExcludedAccounts()
        {
            var list = await accountRepository
                .GetAll()
                .AreNotExcluded()
                .OrderByName()
                .ToListAsync();

            return list.Select(x => new Account(x));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetExcludedAccounts()
        {
            var list = await accountRepository
                .GetAll()
                .AreExcluded()
                .OrderByName()
                .ToListAsync();

            return list.Select(x => new Account(x));
        }

        /// <inheritdoc />
        public async Task SaveAccount(Account account)
        {
            if (account.Data.Id == 0)
            {
                accountRepository.Add(account.Data);
            } else
            {
                accountRepository.Update(account.Data);
            }
            await unitOfWork.Commit();
        }

        /// <inheritdoc />
        public async Task DeleteAccount(Account account)
        {
            accountRepository.Delete(account.Data);
            await unitOfWork.Commit();
        }
    }
}
