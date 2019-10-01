using System;
using System.Collections.Generic;
using MediatR;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class GetCategorySummaryQuery : IRequest<List<CategoryOverviewItem>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
