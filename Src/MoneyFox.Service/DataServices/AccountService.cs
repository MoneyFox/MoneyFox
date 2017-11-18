using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Service.Pocos;
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
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        public AccountService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                                              .OrderByName()
                                              .ToListAsync();

                    return list.Select(x => new Account(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<Account> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return new Account(await dbContext.Accounts
                                                      .Include(x => x.ChargedPayments).ThenInclude(p => p.Category)
                                                      .Include(x => x.ChargedPayments).ThenInclude(p => p.TargetAccount)
                                                      .Include(x => x.TargetedPayments).ThenInclude(p => p.Category)
                                                      .Include(x => x.TargetedPayments)
                                                      .ThenInclude(p => p.ChargedAccount)
                                                      .Include(x => x.ChargedRecurringPayments)
                                                      .ThenInclude(p => p.Category)
                                                      .Include(x => x.ChargedRecurringPayments)
                                                      .ThenInclude(p => p.TargetAccount)
                                                      .Include(x => x.TargetedRecurringPayments)
                                                      .ThenInclude(p => p.Category)
                                                      .Include(x => x.TargetedRecurringPayments)
                                                      .ThenInclude(p => p.ChargedAccount)
                                                      .FirstOrDefaultAsync(x => x.Id == id));
                }
            }
        }

        /// <inheritdoc />
        public async Task<string> GetAccountName(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Accounts.Where(x => x.Id == id).Select(x => x.Name).SingleAsync();
                }
            }
        }

        /// <inheritdoc />
        public Task<int> GetAccountCount()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return dbContext.Accounts.CountAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Accounts
                                                  .NameEquals(name)
                                                  .AnyAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetNotExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                        .AreNotExcluded()
                        .OrderByName()
                        .ToListAsync();

                    return list.Select(x => new Account(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                        .AreExcluded()
                        .OrderByName()
                        .ToListAsync();

                    return list.Select(x => new Account(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task SaveAccount(Account account)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(account.Data).State = account.Data.Id == 0
                        ? EntityState.Added
                        : EntityState.Modified;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteAccount(Account account)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(account.Data).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}
