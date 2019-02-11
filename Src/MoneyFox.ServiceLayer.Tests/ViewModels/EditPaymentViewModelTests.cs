using System;
using System.Diagnostics.CodeAnalysis;
using GenericServices;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Plugin.Messenger;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditPaymentViewModelTests
    {
        [Fact]
        public void Prepare_EndlessRecurringPayment_EndDateNotNull()
        {
            // Arrange
            const int paymentId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();
            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                           .ReturnsAsync(new PaymentViewModel
                           {
                               IsRecurring = true,
                               RecurringPayment = new RecurringPaymentViewModel
                               {
                                   IsEndless = true, EndDate = null
                               }
                           });

            var editAccountVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, new Mock<IMvxMessenger>().Object, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyPaymentParameter(paymentId));

            // Assert
            editAccountVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeTrue();
            editAccountVm.SelectedPayment.RecurringPayment.EndDate.HasValue.ShouldBeTrue();
        }

        [Fact]
        public void Prepare_RecurringPayment_EndDateCorrect()
        {
            // Arrange
            const int paymentId = 99;
            var endDate = DateTime.Today.AddDays(-7);
            var crudServiceMock = new Mock<ICrudServicesAsync>();
            crudServiceMock.Setup(x => x.ReadSingleAsync<PaymentViewModel>(It.IsAny<int>()))
                           .ReturnsAsync(new PaymentViewModel
                           {
                               IsRecurring = true,
                               RecurringPayment = new RecurringPaymentViewModel
                               {
                                   IsEndless = false, EndDate = endDate
                               }
                           });

            var editAccountVm = new EditPaymentViewModel(null, crudServiceMock.Object, null, null, new Mock<IMvxMessenger>().Object, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyPaymentParameter(paymentId));

            // Assert
            editAccountVm.SelectedPayment.RecurringPayment.IsEndless.ShouldBeFalse();
            editAccountVm.SelectedPayment.RecurringPayment.EndDate.ShouldEqual(endDate);
        }
    }
}
