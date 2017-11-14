using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
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
        ///     Selects the account name for the passed id.
        /// </summary>
        /// <param name="id">Id to select the account for.</param>
        /// <returns>The name of the account.</returns>
        Task<string> GetAccountName(int id);

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

    /// <inheritdoc />
    public class AccountService : IAccountService
    {
        private readonly IDbContextScopeFactory dbContextScopeFactory;
        private readonly IAccountRepository accountRepository;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public AccountService(IDbContextScopeFactory dbContextScopeFactory, IAccountRepository accountRepository)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.accountRepository = accountRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var list = await accountRepository
                    .GetAll()
                    .OrderByName()
                    .ToListAsync();

                return list.Select(x => new Account(x));
            }
        }

        /// <inheritdoc />
        public async Task<Account> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                return new Account(await accountRepository.GetById(id));
            }
        }

        /// <inheritdoc />
        public async Task<string> GetAccountName(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                return await accountRepository.GetName(id);
            }
        }

        /// <inheritdoc />
        public Task<int> GetAccountCount()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                return accountRepository.GetAll().CountAsync();
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                return await accountRepository.GetAll()
                                        .NameEquals(name)
                                        .AnyAsync();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetNotExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var list = await accountRepository
                    .GetAll()
                    .AreNotExcluded()
                    .OrderByName()
                    .ToListAsync();

                return list.Select(x => new Account(x));
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var list = await accountRepository
                    .GetAll()
                    .AreExcluded()
                    .OrderByName()
                    .ToListAsync();

                return list.Select(x => new Account(x));
            }
        }

        /// <inheritdoc />
        public async Task SaveAccount(Account account)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                if (account.Data.Id == 0)
                {
                    accountRepository.Add(account.Data);
                }
                else
                {
                    accountRepository.Update(account.Data);
                }
                await dbContextScope.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task DeleteAccount(Account account)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {

                accountRepository.Delete(account.Data);
                await dbContextScope.SaveChangesAsync();
            }
        }
    }
}
