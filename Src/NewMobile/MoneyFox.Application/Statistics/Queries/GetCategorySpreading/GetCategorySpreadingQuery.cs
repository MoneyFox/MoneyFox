using MediatR;
using System;
using System.Collections.Generic;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySpreading
{
    public class GetCategorySpreadingQuery : IRequest<IEnumerable<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
