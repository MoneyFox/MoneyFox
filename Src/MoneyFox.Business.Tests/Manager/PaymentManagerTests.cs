using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Manager
{
    public class PaymentManagerTests
    {
        [Fact]
        public void ClearPayments_IsClearedSetTrue()
        {
            // Setup
            var fixture = new Fixture();
            var account = fixture.Create<AccountViewModel>();

            var payment = fixture.Create<PaymentViewModel>();
            payment.ChargedAccount = account;
            payment.IsCleared = false;
            payment.Date = DateTime.Today.AddDays(-1);

            var accountRepositoryMockSetup = new Mock<IAccountRepository>();
            accountRepositoryMockSetup
                .Setup(x => x.GetList(It.IsAny<Expression<Func<AccountViewModel, bool>>>()))
                .Returns(new List<AccountViewModel> {account});

            var paymentRepositoryMockSetup = new Mock<IPaymentRepository>();
            paymentRepositoryMockSetup
                .Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>()))
                .Returns(new List<PaymentViewModel> { payment });

            var paymentManager = new PaymentManager(paymentRepositoryMockSetup.Object,
                accountRepositoryMockSetup.Object, 
                new Mock<IRecurringPaymentRepository>().Object,
                new Mock<IDialogService>().Object);

            // Execute
            paymentManager.ClearPayments();

            // Assert
            payment.IsCleared.ShouldBeTrue();
        }
    }
}
