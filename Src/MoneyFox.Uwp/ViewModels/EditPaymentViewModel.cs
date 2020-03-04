using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.Services;

namespace MoneyFox.Uwp.ViewModels
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly NavigationService navigationService;
        private readonly IDialogService dialogService;

        [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Intended")]
        public EditPaymentViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService,
                                    NavigationService navigationService) : base(mediator,
                                                                                mapper,
                                                                                dialogService,
                                                                                navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public int PaymentId { get; set; }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncCommand DeleteCommand => new AsyncCommand(DeletePaymentAsync);

        public AsyncCommand InitializeCommand => new AsyncCommand(InitializeAsync);

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SelectedPayment = mapper.Map<PaymentViewModel>(await mediator.Send(new GetPaymentByIdQuery(PaymentId)));
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);

            // We have to set this here since otherwise the end date is null. This causes a crash on android.
            // Also it's user unfriendly if you the default end date is the 1.1.0001.
            if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
                SelectedPayment.RecurringPayment.EndDate = DateTime.Today;

            Title = PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, true);
        }

        protected override async Task SavePaymentAsync()
        {
            try
            {
                var updateRecurring = false;
                if (SelectedPayment.IsRecurring)
                {
                    updateRecurring = await dialogService.ShowConfirmMessageAsync(Strings.ModifyRecurrenceTitle,
                                                                                  Strings.ModifyRecurrenceMessage,
                                                                                  Strings.YesLabel,
                                                                                  Strings.NoLabel);
                }

                var command = new UpdatePaymentCommand(SelectedPayment.Id,
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
                navigationService.GoBack();
            }
            catch (InvalidEndDateException)
            {
                await dialogService.ShowMessageAsync(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }

        private async Task DeletePaymentAsync()
        {
            if (!await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle,
                                                             Strings.DeletePaymentConfirmationMessage,
                                                             Strings.YesLabel,
                                                             Strings.NoLabel)) return;

            var command = new DeletePaymentByIdCommand(SelectedPayment.Id);

            if (SelectedPayment.IsRecurring)
            {
                command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(Strings.DeleteRecurringPaymentTitle,
                                                                                             Strings.DeleteRecurringPaymentMessage);
            }

            await mediator.Send(command);
            navigationService.GoBack();
        }
    }
}
