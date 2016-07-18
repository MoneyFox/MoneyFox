using System;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Account> AccountRepository { get; }

        IPaymentRepository PaymentRepository { get; }
        IRepository<RecurringPayment> RecurringPaymentRepository { get; }

        IRepository<Category> CategoryRepository { get; }
    }
}