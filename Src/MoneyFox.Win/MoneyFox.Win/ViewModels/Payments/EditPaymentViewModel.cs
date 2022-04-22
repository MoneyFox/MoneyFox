namespace MoneyFox.Win.ViewModels.Payments;

using System;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Commands.Payments.DeletePaymentById;
using Core.Commands.Payments.UpdatePayment;
using Core.Common.Exceptions;
using Core.Common.Interfaces;
using Core.Queries;
using Core.Resources;
using MediatR;
using Microsoft.AppCenter.Crashes;
using Serilog;
using Services;
using Utilities;

public class EditPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    public EditPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        navigationService: navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public AsyncRelayCommand DeleteCommand => new(DeletePaymentAsync);

    public RelayCommand<int> InitializeCommand => new(async paymentId => await InitializeAsync(paymentId));

    protected async Task InitializeAsync(int paymentId)
    {
        try
        {
            await base.InitializeAsync();
            SelectedPayment = mapper.Map<PaymentViewModel>(await mediator.Send(new GetPaymentByIdQuery(paymentId)));
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);

            // We have to set this here since otherwise the end date is null. This causes a crash on android.
            // Also it's user unfriendly if you the default end date is the 1.1.0001.
            if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment != null && SelectedPayment.RecurringPayment.IsEndless)
            {
                SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
            }

            Title = PaymentTypeHelper.GetViewTitleForType(type: SelectedPayment.Type, isEditMode: true);
        }
        catch (PaymentNotFoundException ex)
        {
            Crashes.TrackError(ex);
        }
    }

    protected override async Task SavePaymentAsync()
    {
        try
        {
            IsBusy = true;
            var updateRecurring = false;
            if (SelectedPayment.IsRecurring)
            {
                updateRecurring = await dialogService.ShowConfirmMessageAsync(
                    title: Strings.ModifyRecurrenceTitle,
                    message: Strings.ModifyRecurrenceMessage,
                    positiveButtonText: Strings.YesLabel,
                    negativeButtonText: Strings.NoLabel);
            }

            var command = new UpdatePaymentCommand(
                id: SelectedPayment.Id,
                date: SelectedPayment.Date,
                amount: SelectedPayment.Amount,
                isCleared: SelectedPayment.IsCleared,
                type: SelectedPayment.Type,
                note: SelectedPayment.Note,
                isRecurring: SelectedPayment.IsRecurring,
                categoryId: SelectedPayment.Category?.Id ?? 0,
                chargedAccountId: SelectedPayment.ChargedAccount?.Id ?? 0,
                targetAccountId: SelectedPayment.TargetAccount?.Id ?? 0,
                updateRecurringPayment: updateRecurring,
                recurrence: SelectedPayment.RecurringPayment?.Recurrence,
                isEndless: SelectedPayment.RecurringPayment?.IsEndless,
                endDate: SelectedPayment.RecurringPayment?.EndDate,
                isLastDayOfMonth: SelectedPayment.RecurringPayment?.IsLastDayOfMonth ?? false);

            await mediator.Send(command);
        }
        catch (InvalidEndDateException)
        {
            await dialogService.ShowMessageAsync(title: Strings.InvalidEnddateTitle, message: Strings.InvalidEnddateMessage);
        }
    }

    private async Task DeletePaymentAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Strings.DeleteTitle, message: Strings.DeletePaymentConfirmationMessage))
        {
            var command = new DeletePaymentByIdCommand(SelectedPayment.Id);
            if (SelectedPayment.IsRecurring)
            {
                command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                    title: Strings.DeleteRecurringPaymentTitle,
                    message: Strings.DeleteRecurringPaymentMessage);
            }

            try
            {
                IsBusy = true;
                await mediator.Send(command);
                navigationService.GoBack();
            }
            catch (PaymentNotFoundException ex)
            {
                Log.Warning(exception: ex, messageTemplate: "Error during payment deletion");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
