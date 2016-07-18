using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class BalanceViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [TestMethod]
        public void GetTotalBalance_Zero()
        {
            var unitOfWorkSetup = new Mock<IUnitOfWork>();

            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.Data).Returns(() => new ObservableCollection<Payment>());

            unitOfWorkSetup.SetupGet(x => x.PaymentRepository).Returns(paymentMockSetup.Object);
            unitOfWorkSetup.SetupGet(x => x.AccountRepository).Returns(new Mock<IRepository<Account>>().Object);

            var vm = new BalanceViewModel(unitOfWorkSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(0);
        }

        [TestMethod]
        public void GetTotalBalance_TwoExpense_SumOfPayments()
        {
            var unitOfWorkSetup = new Mock<IUnitOfWork>();

            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.Data)
                .Returns(() => new ObservableCollection<Payment>
                {
                    new Payment {Amount = 20, Type = (int) PaymentType.Expense},
                    new Payment {Amount = 60, Type = (int) PaymentType.Expense}
                });

            unitOfWorkSetup.SetupGet(x => x.PaymentRepository).Returns(paymentMockSetup.Object);
            unitOfWorkSetup.SetupGet(x => x.AccountRepository).Returns(new Mock<IRepository<Account>>().Object);

            var vm = new BalanceViewModel(unitOfWorkSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(-80);
        }

        [TestMethod]
        public void GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var unitOfWorkSetup = new Mock<IUnitOfWork>();

            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.Data)
                .Returns(() => new ObservableCollection<Payment>
                {
                    new Payment {Amount = 20, Type = (int) PaymentType.Expense},
                    new Payment {Amount = 60, Type = (int) PaymentType.Income}
                });

            unitOfWorkSetup.SetupGet(x => x.PaymentRepository).Returns(paymentMockSetup.Object);
            unitOfWorkSetup.SetupGet(x => x.AccountRepository).Returns(new Mock<IRepository<Account>>().Object);

            var vm = new BalanceViewModel(unitOfWorkSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(40);
        }

        [TestMethod]
        public void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var unitOfWorkSetup = new Mock<IUnitOfWork>();

            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.Data).Returns(() => new ObservableCollection<Payment>());

            var accountMockSetup = new Mock<IRepository<Account>>();
            accountMockSetup.SetupGet(x => x.Data).Returns(() => new ObservableCollection<Account>
            {
                new Account {CurrentBalance = 500},
                new Account {CurrentBalance = 200}
            });

            unitOfWorkSetup.SetupGet(x => x.PaymentRepository).Returns(paymentMockSetup.Object);
            unitOfWorkSetup.SetupGet(x => x.AccountRepository).Returns(accountMockSetup.Object);

            var vm = new BalanceViewModel(unitOfWorkSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(700);
            vm.EndOfMonthBalance.ShouldBe(700);
        }
    }
}