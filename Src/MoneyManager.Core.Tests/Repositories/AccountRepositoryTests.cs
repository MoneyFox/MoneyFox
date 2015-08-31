using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    public class AccountRepositoryTests
    {
        [Theory]
        [InlineData("Sparkonto", "Sparkonto")]
        [InlineData("", "[No Name]")]
        public void Save_InputName_CorrectNameAssigned(string nameInput, string nameExpected)
        {
            var testList = new List<Account>();

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.Save(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var accountRepository = new AccountRepository(accountDataAccessSetup.Object);

            var account = new Account
            {
                Name = nameInput,
                CurrentBalance = 6034
            };

            accountRepository.Save(account);

            testList[0].ShouldBeSameAs(account);
            testList[0].Name.ShouldBe(nameExpected);
        }

        [Fact]
        public void AccessCache()
        {
            new AccountRepository(new AccountDataAccessMock()).Data.ShouldNotBeNull();
        }

        [Fact]
        public void Delete_None_AccountDeleted()
        {
            var testList = new List<Account>();

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.Save(It.IsAny<Account>()))
                .Callback((Account acc) => testList.Add(acc));
        
            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalance = 6034
            };

            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>
            {
                account
            });

            var repository = new AccountRepository(accountDataAccessSetup.Object);

            repository.Delete(account);

            testList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }
    }
}