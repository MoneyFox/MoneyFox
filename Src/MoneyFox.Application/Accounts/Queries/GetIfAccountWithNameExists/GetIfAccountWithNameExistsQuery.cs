﻿using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists
{
    public class GetIfAccountWithNameExistsQuery : IRequest<bool>
    {
        public GetIfAccountWithNameExistsQuery(string accountName)
        {
            AccountName = accountName;
        }

        public string AccountName { get; }

        public class Handler : IRequestHandler<GetIfAccountWithNameExistsQuery, bool>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<bool> Handle(GetIfAccountWithNameExistsQuery request, CancellationToken cancellationToken)
                => await contextAdapter.Context.Accounts.AnyWithNameAsync(request.AccountName);
        }
    }
}