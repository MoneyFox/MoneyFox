using MoneyFox.Business.Tests.Fixtures;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Tests;
using MoneyFox.Service.DataServices;
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
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object)
                .GoToAddIncomeCommand.Execute(PaymentType.Income.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Income");
        }

        [Fact]
        public void GoToAddPayment_ExpenseNoEdit_CorrectParameterPassed()
        {
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object)
                .GoToAddExpenseCommand.Execute(PaymentType.Expense.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Expense");
        }

        [Fact]
        public void GoToAddPayment_TransferNoEdit_CorrectParameterPassed()
        {
            new AccountListViewActionViewModel(new Mock<IAccountService>().Object)
                .GoToAddTransferCommand.Execute(PaymentType.Transfer.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Transfer");
        }

        [Fact]
        public void IsAddIncomeEnabled_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsAddIncomeAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddIncomeEnabled_OneAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsAddIncomeAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsAddExpenseEnabled_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(0);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsAddExpenseAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsAddExpenseEnabled_OneAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsAddExpenseAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsTransferAvailable_EmptyData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(0);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_OneAccountInData_NotAvailable()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(1);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [Fact]
        public void IsTransferAvailable_TwoAccountInData_Available()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountCount())
                .ReturnsAsync(2);

            new AccountListViewActionViewModel(accountServiceMock.Object).IsTransferAvailable.ShouldBeTrue();
        }
    }
}