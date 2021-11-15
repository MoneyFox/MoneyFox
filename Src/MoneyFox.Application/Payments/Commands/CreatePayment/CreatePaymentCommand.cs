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
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest
    {
        public CreatePaymentCommand(Payment paymentToSave)
        {
            PaymentToSave = paymentToSave;
        }

        public Payment PaymentToSave { get; }

        public class Handler : IRequestHandler<CreatePaymentCommand>
        {
            private readonly Logger logger = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;
            private readonly IBackupService backupService;
            private readonly ISettingsFacade settingsFacade;

            public Handler(IContextAdapter contextAdapter,
                IBackupService backupService,
                ISettingsFacade settingsFacade)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
                this.settingsFacade = settingsFacade;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                contextAdapter.Context.Entry(request.PaymentToSave).State = EntityState.Added;
                contextAdapter.Context.Entry(request.PaymentToSave.ChargedAccount).State = EntityState.Modified;

                if(request.PaymentToSave.TargetAccount != null)
                {
                    contextAdapter.Context.Entry(request.PaymentToSave.TargetAccount).State = EntityState.Modified;
                }

                if(request.PaymentToSave.IsRecurring)
                {
                    if(request.PaymentToSave.RecurringPayment == null)
                    {
                        var exception = new RecurringPaymentNullException(
                            $"Recurring Payment for Payment {request.PaymentToSave.Id} is null, although payment is marked recurring.");
                        logger.Error(exception);
                        throw exception;
                    }

                    contextAdapter.Context.Entry(request.PaymentToSave.RecurringPayment).State = EntityState.Added;
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                backupService.UploadBackupAsync().FireAndForgetSafeAsync();

                return Unit.Value;
            }
        }
    }
}