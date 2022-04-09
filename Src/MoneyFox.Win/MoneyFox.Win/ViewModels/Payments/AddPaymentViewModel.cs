namespace MoneyFox.Win.ViewModels.Payments;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Exceptions;
using Core.Aggregates.Payments;
using Core.Commands.Payments.CreatePayment;
using Core.Common.Interfaces;
using Core.Queries;
using Core.Resources;
using MediatR;
using Services;
using Utilities;

public class AddPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IDialogService dialogService;

    public AddPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        navigationService: navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
    }

    public PaymentType PaymentType { get; set; }

    public RelayCommand InitializeCommand => new(async () => await InitializeAsync());

    protected override async Task InitializeAsync()
    {
        Title = PaymentTypeHelper.GetViewTitleForType(type: PaymentType, isEditMode: false);
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);
        SelectedPayment.Type = PaymentType;
        await base.InitializeAsync();
        SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();
        if (SelectedPayment.IsTransfer)
        {
            SelectedItemChangedCommand.Execute(null);
            SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
        }
    }

    protected override async Task SavePaymentAsync()
    {
        try
        {
            IsBusy = true;
            var payment = new Payment(
                date: SelectedPayment.Date,
                amount: SelectedPayment.Amount,
                type: SelectedPayment.Type,
                chargedAccount: await mediator.Send(new GetAccountByIdQuery(SelectedPayment.ChargedAccount.Id)),
                targetAccount: SelectedPayment.TargetAccount != null ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id)) : null,
                category: mapper.Map<Category>(SelectedPayment.Category),
                note: SelectedPayment.Note);

            if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment != null)
            {
                payment.AddRecurringPayment(
                    recurrence: SelectedPayment.RecurringPayment.Recurrence,
                    endDate: SelectedPayment.RecurringPayment.IsEndless ? null : SelectedPayment.RecurringPayment.EndDate);
            }

            await mediator.Send(new CreatePaymentCommand(payment));
        }
        catch (InvalidEndDateException)
        {
            await dialogService.ShowMessageAsync(title: Strings.InvalidEnddateTitle, message: Strings.InvalidEnddateMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
