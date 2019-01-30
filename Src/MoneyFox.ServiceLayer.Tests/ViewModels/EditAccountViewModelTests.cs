using GenericServices;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    public class EditAccountViewModelTests
    {
        [Fact]
        public void Prepare_AccountLoaded()
        {
            // Arrange
            const int accoundId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var editAccountVm = new EditAccountViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyAccountParameter(accoundId));

            // Assert
            crudServiceMock.Verify(x => x.ReadSingleAsync<AccountViewModel>(accoundId), Times.Once);
        }
    }
}
