﻿using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Utilities;
using NLog;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Payments
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        public EditPaymentViewModel(IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            INavigationService navigationService) : base(
            mediator,
            mapper,
            dialogService,
            navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncRelayCommand DeleteCommand => new AsyncRelayCommand(DeletePaymentAsync);

        public RelayCommand<int> InitializeCommand =>
            new RelayCommand<int>(async paymentId => await InitializeAsync(paymentId));

        protected async Task InitializeAsync(int paymentId)
        {
            try
            {
                await base.InitializeAsync();

                SelectedPayment = mapper.Map<PaymentViewModel>(await mediator.Send(new GetPaymentByIdQuery(paymentId)));
                AmountString = HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);

                // We have to set this here since otherwise the end date is null. This causes a crash on android.
                // Also it's user unfriendly if you the default end date is the 1.1.0001.
                if(SelectedPayment.IsRecurring
                   && SelectedPayment.RecurringPayment != null
                   && SelectedPayment.RecurringPayment.IsEndless)
                {
                    SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
                }

                Title = PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, true);
            }
            catch(PaymentNotFoundException ex)
            {
                Crashes.TrackError(ex);
            }
        }

        protected override async Task SavePaymentAsync()
        {
            try
            {
                bool updateRecurring = false;
                if(SelectedPayment.IsRecurring)
                {
                    updateRecurring = await dialogService.ShowConfirmMessageAsync(
                        Strings.ModifyRecurrenceTitle,
                        Strings.ModifyRecurrenceMessage,
                        Strings.YesLabel,
                        Strings.NoLabel);
                }

                var command = new UpdatePaymentCommand(
                    SelectedPayment.Id,
                    SelectedPayment.Date,
                    SelectedPayment.Amount,
                    SelectedPayment.IsCleared,
                    SelectedPayment.Type,
                    SelectedPayment.Note,
                    SelectedPayment.IsRecurring,
                    SelectedPayment.Category != null
                        ? SelectedPayment.Category.Id
                        : 0,
                    SelectedPayment.ChargedAccount != null
                        ? SelectedPayment.ChargedAccount.Id
                        : 0,
                    SelectedPayment.TargetAccount != null
                        ? SelectedPayment.TargetAccount.Id
                        : 0,
                    updateRecurring,
                    SelectedPayment.RecurringPayment?.Recurrence,
                    SelectedPayment.RecurringPayment?.IsEndless,
                    SelectedPayment.RecurringPayment?.EndDate);

                await mediator.Send(command);
            }
            catch(InvalidEndDateException)
            {
                await dialogService.ShowMessageAsync(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }

        private async Task DeletePaymentAsync()
        {
            if(await dialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeletePaymentConfirmationMessage))
            {
                var command = new DeletePaymentByIdCommand(SelectedPayment.Id);

                if(SelectedPayment.IsRecurring)
                {
                    command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                        Strings.DeleteRecurringPaymentTitle,
                        Strings.DeleteRecurringPaymentMessage);
                }

                try
                {
                    await dialogService.ShowLoadingDialogAsync();
                    await mediator.Send(command);
                    navigationService.GoBack();
                }
                catch(PaymentNotFoundException ex)
                {
                    logger.Warn(ex);
                }
                finally
                {
                    await dialogService.HideLoadingDialogAsync();
                }
            }
        }
    }
}