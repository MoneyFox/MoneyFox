﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.DbBackup;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Commands.DeletePaymentById
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
            private readonly IBackupService backupService;
            private readonly ISettingsFacade settingsFacade;

            public Handler(IContextAdapter contextAdapter, IBackupService backupService, ISettingsFacade settingsFacade)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
                this.settingsFacade = settingsFacade;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken)
            {
                await backupService.RestoreBackupAsync();

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

                settingsFacade.LastDatabaseUpdate = DateTime.Now;

                try
                {
                    backupService.UploadBackupAsync().FireAndForgetSafeAsync();
                }
                catch(NetworkConnectionException)
                {
                    logManager.Info("No Network Connection. Couldn't Upload backup.");
                }

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