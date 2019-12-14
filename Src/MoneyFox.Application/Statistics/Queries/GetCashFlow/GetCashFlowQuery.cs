using System;
using System.Collections.Generic;
using MediatR;

namespace MoneyFox.Application.Statistics.Queries.GetCashFlow
{
    public class GetCashFlowQuery : IRequest<List<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
