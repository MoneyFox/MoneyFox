namespace MoneyFox.ViewModels.Payments
{

    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.ApplicationCore.Queries;
    using Core.Commands.Payments.CreatePayment;
    using Core.Common.Interfaces;
    using JetBrains.Annotations;
    using MediatR;

    [UsedImplicitly]
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public AddPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(
            mediator: mediator,
            mapper: mapper,
            dialogService: dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();
        }

        protected override async Task SavePaymentAsync()
        {
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
                    isLastDayOfMonth: SelectedPayment.RecurringPayment.IsLastDayOfMonth,
                    endDate: SelectedPayment.RecurringPayment.IsEndless ? null : SelectedPayment.RecurringPayment.EndDate);
            }

            await mediator.Send(new CreatePaymentCommand(payment));
        }
    }

}
