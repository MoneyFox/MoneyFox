﻿using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        public EditPaymentViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService)
            : base(mediator, mapper, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        public async Task InitializeAsync(int paymentId)
        {
            await base.InitializeAsync();
            SelectedPayment = mapper.Map<PaymentViewModel>(await mediator.Send(new GetPaymentByIdQuery(paymentId)));
        }

        public RelayCommand<PaymentViewModel> DeleteCommand
            => new RelayCommand<PaymentViewModel>(async (p) => await DeletePayment(p));

        protected override async Task SavePaymentAsync()
        {
            bool updateRecurring = false;
            if(SelectedPayment.IsRecurring)
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
        }

        private async Task DeletePayment(PaymentViewModel payment)
        {
            if(await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
            {
                await mediator.Send(new DeletePaymentByIdCommand(payment.Id));
                await Shell.Current.Navigation.PopModalAsync();
            }
        }
    }
}
