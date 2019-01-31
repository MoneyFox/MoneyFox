using System.Diagnostics.CodeAnalysis;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Plugin.Messenger;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddPaymentViewModelTests
    {
        [Fact]
        public void Prepare_PaymentCreated()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addPaymentVm = new AddPaymentViewModel(null,
                                                       crudServiceMock.Object,
                                                       null, null,
                                                       new Mock<IMvxMessenger>().Object,
                                                       null, null, null);

            // Act
            addPaymentVm.Prepare(new ModifyPaymentParameter());

            // Assert
            addPaymentVm.SelectedPayment.ShouldNotBeNull();
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addPaymentVm = new AddPaymentViewModel(null,
                                                       crudServiceMock.Object,
                                                       null, null,
                                                       new Mock<IMvxMessenger>().Object,
                                                       null, null, null);
            // Act
            addPaymentVm.Prepare(new ModifyPaymentParameter());

            // Assert
            addPaymentVm.Title.Contains(Strings.AddTitle);
        }
    }
}