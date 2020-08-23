using FluentAssertions;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapter;

        public GetIncludedAccountBalanceSummaryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded1 = new Account("test", 100);
            var accountIncluded2 = new Account("test", 120);

            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded1);
            await context.AddAsync(accountIncluded2);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapter)
                   .Handle(new GetIncludedAccountBalanceSummaryQuery(), default);

            // Assert
            result.Should().Be(220);
        }
    }
}
