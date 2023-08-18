namespace MoneyFox.Core.Features._Legacy_.Payments.CreatePayment;

using System.Threading;
using System.Threading.Tasks;
using Aptabase.Maui;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreatePaymentCommand : IRequest
{
    public CreatePaymentCommand(Payment paymentToSave)
    {
        PaymentToSave = paymentToSave;
    }

    private Payment PaymentToSave { get; }

    public class Handler : IRequestHandler<CreatePaymentCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IAptabaseClient aptabaseClient;

        public Handler(IAppDbContext appDbContext, IAptabaseClient aptabaseClient)
        {
            this.appDbContext = appDbContext;
            this.aptabaseClient = aptabaseClient;
        }

        public async Task Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            appDbContext.Entry(request.PaymentToSave).State = EntityState.Added;
            appDbContext.Entry(request.PaymentToSave.ChargedAccount).State = EntityState.Modified;
            if (request.PaymentToSave.TargetAccount != null)
            {
                appDbContext.Entry(request.PaymentToSave.TargetAccount).State = EntityState.Modified;
            }

            await appDbContext.SaveChangesAsync(cancellationToken);
            aptabaseClient.TrackEvent("payment_created");
        }
    }
}
