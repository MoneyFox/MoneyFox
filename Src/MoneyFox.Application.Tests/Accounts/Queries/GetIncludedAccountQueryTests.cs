using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccount;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIncludedAccountQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetIncludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            List<Account> result = await new GetIncludedAccountQuery.Handler(contextAdapterMock.Object).Handle(new GetIncludedAccountQuery(), default);

            // Assert
            Assert.Single(result);
        }
    }
}
