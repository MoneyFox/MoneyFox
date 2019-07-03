using MediatR;
using MoneyFox.Application.Statistics.Models;
using System;
using System.Collections.Generic;

namespace MoneyFox.Application.Statistics.Queries
{
    public class GetCashFlowQuery : IRequest<List<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
