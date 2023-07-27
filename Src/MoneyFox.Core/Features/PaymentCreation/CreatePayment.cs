namespace MoneyFox.Core.Features.PaymentCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;
using MediatR;

internal static class CreatePayment
{
    public record Command(
        int Id,
        int ChargedAccount,
        int? TargetAccount,
        Money Amount,
        int? CategoryId,
        PaymentType Type,
        DateOnly StartDate,
        DateOnly? EndDate,
        Recurrence Recurrence,
        string? Note,
        bool IsLastDayOfMonth) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        public Task Handle(Command command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
