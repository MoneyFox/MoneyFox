using System.Threading.Tasks;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Enables to save changes made over several repositories in one transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Returns an <see cref="IAccountRepository"/>.
        ///     If it wasn't set, it is created.
        /// </summary>
        IAccountRepository AccountRepository { get; set; }

        /// <summary>
        ///     Returns an <see cref="ICategoryRepository"/>.
        ///     If it wasn't set, it is created.
        /// </summary>
        ICategoryRepository CategoryRepository { get; set; }

        /// <summary>
        ///     Returns an <see cref="IPaymentRepository"/>.
        ///     If it wasn't set, it is created.
        /// </summary>
        IPaymentRepository PaymentRepository { get; set; }

        /// <summary>
        ///     Returns an <see cref="IRecurringPaymentRepository"/>.
        ///     If it wasn't set, it is created.
        /// </summary>
        IRecurringPaymentRepository RecurringPaymentRepository { get; set; }


        /// <summary>
        ///     Saves all pending changes to the database.
        /// </summary>
        Task Commit();
    }

    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="dbFactory">DbFactory to get Context.</param>
        public UnitOfWork(IDbFactory dbFactory)
        {
            DbContext = dbFactory.Init().Result;
        }

        private ApplicationContext DbContext { get; }

        #region Repositories

        private IAccountRepository accountRepository;
        private ICategoryRepository categoryRepository;
        private IPaymentRepository paymentRepository;
        private IRecurringPaymentRepository recurringPaymentRepository;

        /// <inheritdoc />
        public IAccountRepository AccountRepository
        {
            get => accountRepository ?? (accountRepository = new AccountRepository(DbContext));
            set => accountRepository = value;
        }

        /// <inheritdoc />
        public ICategoryRepository CategoryRepository
        {
            get => categoryRepository ?? (categoryRepository = new CategoryRepository(DbContext));
            set => categoryRepository = value;
        }

        /// <inheritdoc />
        public IPaymentRepository PaymentRepository
        {
            get => paymentRepository ?? (paymentRepository = new PaymentRepository(DbContext));
            set => paymentRepository = value;
        }

        /// <inheritdoc />
        public IRecurringPaymentRepository RecurringPaymentRepository
        {
            get => recurringPaymentRepository ?? (recurringPaymentRepository = new RecurringPaymentRepository(DbContext));
            set => recurringPaymentRepository = value;
        }

        #endregion

        /// <inheritdoc />
        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}