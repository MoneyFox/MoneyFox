using System.Collections.Generic;
using MoneyFox.Business.Tests.Fixtures;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [Collection("MvxIocCollection")]
    public class AccountListViewActionModelTests : MvxIocFixture
    {
        [Fact]
        public void GoToAddPayment_IncomeNoEdit_CorrectParameterPassed()
        {
            new AccountListViewActionViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddIncomeCommand.Execute(PaymentType.Income.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Income");
        }

        [Fact]
        public void GoToAddPayment_ExpenseNoEdit_CorrectParameterPassed()
        {
            new AccountListViewActionViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddExpenseCommand.Execute(PaymentType.Expense.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Expense");
        }

        [Fact]
        public void GoToAddPayment_TransferNoEdit_CorrectParameterPassed()
        {
            new AccountListViewActionViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddTransferCommand.Execute(PaymentType.Transfer.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Transfer");
        }

        [Fact]
        public void IsAddIncomeEnabled_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsAddIncomeAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddIncomeEnabled_OneAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>()
                {
                    new AccountViewModel()
                });

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsAddIncomeAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsAddExpenseEnabled_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsAddExpenseAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddExpenseEnabled_OneAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>()
                {
                    new AccountViewModel()
                });

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsAddExpenseAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsTransferAvailable_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_OneAccountInData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>
                {
                    new AccountViewModel()
                });

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_TwoAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>
                {
                    new AccountViewModel(),
                    new AccountViewModel()
                });

            new AccountListViewActionViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeTrue();
        }
    }
}