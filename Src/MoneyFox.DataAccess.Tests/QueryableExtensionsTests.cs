using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper.QueryableExtensions;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;
using Ploeh.AutoFixture;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.DataAccess.Tests
{
    public class QueryableExtensionsTests
    {
        public QueryableExtensionsTests()
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();
        }

        [Fact]
        public void ProjectTo_ModelToModel()
        {
            var fixture = new Fixture();
            var account = fixture.Create<Account>();

            var accountViewModel = new List<Account> {account}
                .AsQueryable()
                .ProjectTo<AccountViewModel>()
                .ToList();

            accountViewModel[0].Id.ShouldBe(account.Id);
            accountViewModel[0].Name.ShouldBe(account.Name);
            accountViewModel[0].CurrentBalance.ShouldBe(account.CurrentBalance);
            accountViewModel[0].IsOverdrawn.ShouldBe(account.IsOverdrawn);
            accountViewModel[0].Iban.ShouldBe(account.Iban);
            accountViewModel[0].Note.ShouldBe(account.Note);
        }
    }
}
