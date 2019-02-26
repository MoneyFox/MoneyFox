using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using TestSupport.EfHelpers;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Services
{
    public class PaymentServiceTests
    {
        private Mock<IModifyPaymentAction> modifyPaymentActionMock;
        private Mock<IDialogService> dialogServiceMock;

        public PaymentServiceTests()
        {
            modifyPaymentActionMock = new Mock<IModifyPaymentAction>();
            dialogServiceMock = new Mock<IDialogService>();
        }

        [Fact]
        public async Task SavePayment_AddFailed_ResultFailed()
        {
            // Arrange
            modifyPaymentActionMock.Setup(x => x.AddPayment(It.IsAny<Payment>()))
                .ReturnsAsync(OperationResult.Failed(""));

            var dbOptions = this.CreateUniqueClassOptions<EfCoreContext>();

            using (var context = new EfCoreContext(dbOptions))
            {
                var paymentService = new PaymentService(context, modifyPaymentActionMock.Object, dialogServiceMock.Object);

                // Act
                await paymentService.SavePayment(new PaymentViewModel());

                // Assert
            }
        }
    }
}
