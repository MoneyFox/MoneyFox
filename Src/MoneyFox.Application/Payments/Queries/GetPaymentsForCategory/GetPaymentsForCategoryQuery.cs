using MediatR;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetPaymentsForCategory
{
    public class GetPaymentsForCategoryQuery : IRequest<List<Payment>>
    {
        public GetPaymentsForCategoryQuery(int categoryId, DateTime dateRangeFrom, DateTime dateRangeTo)
        {
            CategoryId = categoryId;
            DateRangeFrom = dateRangeFrom;
            DateRangeTo = dateRangeTo;
        }

        public int CategoryId { get; set; }
        public DateTime DateRangeFrom { get; set; }
        public DateTime DateRangeTo { get; set; }

        public class Handler : IRequestHandler<GetPaymentsForCategoryQuery, List<Payment>>
        {
            public Task<List<Payment>> Handle(GetPaymentsForCategoryQuery request, CancellationToken cancellationToken) => throw new NotImplementedException();
        }
    }
}
