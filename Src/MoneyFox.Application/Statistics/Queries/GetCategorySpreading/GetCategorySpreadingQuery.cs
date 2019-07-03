using System;
using System.Collections.Generic;
using MediatR;
using MoneyFox.Application.Statistics.Models;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySpreading
{
    public class GetCategorySpreadingQuery : IRequest<IEnumerable<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
