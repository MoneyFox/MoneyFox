using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Payments.DeletePaymentById
{
    public class DeletePaymentByIdCommand : IRequest
    {
        public DeletePaymentByIdCommand(int paymentId, bool deleteRecurringPayment = false)
        {
            PaymentId = paymentId;
            DeleteRecurringPayment = deleteRecurringPayment;
        }

        public int PaymentId { get; }

        public bool DeleteRecurringPayment { get; set; }

        public class Handler : IRequestHandler<DeletePaymentByIdCommand>
        {
            private readonly Logger logManager = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken)
            {
                Payment? entityToDelete = await contextAdapter.Context
                                                              .Payments
                                                              .Include(x => x.ChargedAccount)
                                                              .Include(x => x.TargetAccount)
                                                              .Include(x => x.RecurringPayment)
                                                              .SingleOrDefaultAsync(
                                                                  x => x.Id == request.PaymentId,
                                                                  cancellationToken);

                if(entityToDelete == null)
                {
                    throw new PaymentNotFoundException();
                }

                entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
                entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);

                if(request.DeleteRecurringPayment && entityToDelete.RecurringPayment != null)
                {
                    await DeleteRecurringPaymentAsync(entityToDelete.RecurringPayment.Id);
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                contextAdapter.Context.Payments.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }

            private async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
            {
                List<Payment> payments = await contextAdapter.Context
                                                             .Payments
                                                             .Where(x => x.IsRecurring)
                                                             .Where(x => x.RecurringPayment!.Id == recurringPaymentId)
                                                             .ToListAsync();

                payments.ForEach(x => x.RemoveRecurringPayment());
                contextAdapter.Context.RecurringPayments
                              .Remove(
                                  await contextAdapter.Context.RecurringPayments
                                                      .FindAsync(recurringPaymentId));
            }
        }
    }
}