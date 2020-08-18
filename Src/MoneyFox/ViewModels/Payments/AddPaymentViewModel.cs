using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Payments.Commands.CreatePayment;
using MoneyFox.Domain.Entities;
using MoneyFox.Services;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Payments
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddPaymentViewModel(IMediator mediator,
                                   IMapper mapper,
                                   IDialogService dialogService)
            : base(mediator, mapper, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task SavePaymentAsync()
        {
            var payment = new Payment(SelectedPayment.Date,
                                        SelectedPayment.Amount,
                                        SelectedPayment.Type,
                                        await mediator.Send(new GetAccountByIdQuery(SelectedPayment.ChargedAccount.Id)),
                                        SelectedPayment.TargetAccount != null
                                            ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id))
                                            : null,
                                        mapper.Map<Category>(SelectedPayment.Category),
                                        SelectedPayment.Note);

            if(SelectedPayment.IsRecurring)
                payment.AddRecurringPayment(SelectedPayment.RecurringPayment.Recurrence, SelectedPayment.RecurringPayment.EndDate);

            await mediator.Send(new CreatePaymentCommand(payment));
        }
    }
}
