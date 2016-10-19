using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Tests.Mocks;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;
using System.Collections.Generic;
using MoneyFox.Shared.Interfaces.Repositories;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests : MvxIoCSupportingTest
    {
        protected MockDispatcher MockDispatcher { get; private set; }

        [TestInitialize]
        public void Init()
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

        [TestMethod]
        public void GoToAddPayment_IncomeNoEdit_CorrectParameterPassed()
        {
            new MainViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddPaymentCommand.Execute(PaymentType.Income.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Income");
        }

        [TestMethod]
        public void GoToAddPayment_ExpenseNoEdit_CorrectParameterPassed()
        {
            new MainViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddPaymentCommand.Execute(PaymentType.Expense.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Expense");
        }

        [TestMethod]
        public void GoToAddPayment_TransferNoEdit_CorrectParameterPassed()
        {
            new MainViewModel(new Mock<IAccountRepository>().Object)
                .GoToAddPaymentCommand.Execute(PaymentType.Transfer.ToString());

            MockDispatcher.Requests.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ViewModelType.ShouldBe(typeof(ModifyPaymentViewModel));
            MockDispatcher.Requests[0].ParameterValues.Count.ShouldBe(1);
            MockDispatcher.Requests[0].ParameterValues["type"].ShouldBe("Transfer");
        }

        [TestMethod]
        public void IsAddIncomeEnabled_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new MainViewModel(accountRepositoryMock.Object).IsAddIncomeAvailable.ShouldBeFalse();
        }

        [TestMethod]
        public void IsAddIncomeEnabled_OneAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>()
                {
                    new AccountViewModel()
                });

            new MainViewModel(accountRepositoryMock.Object).IsAddIncomeAvailable.ShouldBeTrue();
        }

        [TestMethod]
        public void IsAddExpenseEnabled_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new MainViewModel(accountRepositoryMock.Object).IsAddExpenseAvailable.ShouldBeFalse();
        }

        [TestMethod]
        public void IsAddExpenseEnabled_OneAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>()
                {
                    new AccountViewModel()
                });

            new MainViewModel(accountRepositoryMock.Object).IsAddExpenseAvailable.ShouldBeTrue();
        }

        [TestMethod]
        public void IsTransferAvailable_EmptyData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>());

            new MainViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [TestMethod]
        public void IsTransferAvailable_OneAccountInData_NotAvailable()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>
                {
                    new AccountViewModel()
                });

            new MainViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeFalse();
        }

        [TestMethod]
        public void IsTransferAvailable_TwoAccountInData_Available()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(x => x.GetList(null))
                .Returns(new List<AccountViewModel>
                {
                    new AccountViewModel(),
                    new AccountViewModel()
                });

            new MainViewModel(accountRepositoryMock.Object).IsTransferAvailable.ShouldBeTrue();
        }
    }
}