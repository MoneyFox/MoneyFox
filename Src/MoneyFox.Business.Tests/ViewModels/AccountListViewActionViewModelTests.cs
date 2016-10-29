using System.Collections.Generic;
using MoneyFox.Business.Tests.Mocks;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using Moq;
using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class AccountListViewActionViewModelTests : MvxIoCSupportingTest
    {
        protected MockDispatcher MockDispatcher { get; private set; }

        public AccountListViewActionViewModelTests()
        {
            Setup();
        }

        /// <summary>
        ///     This is needed for the navigation to work in the test.
        /// </summary>
        protected override void AdditionalSetup()
        {
            MockDispatcher = new MockDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
            Ioc.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
        }

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