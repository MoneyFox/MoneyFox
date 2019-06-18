using System.Diagnostics.CodeAnalysis;
using GenericServices;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditAccountViewModelTests
    {
        [Fact]
        public void Prepare_AccountLoaded()
        {
            // Arrange
            const int accountId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var editAccountVm = new EditAccountViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyAccountParameter(accountId));

            // Assert
            crudServiceMock.Verify(x => x.ReadSingleAsync<AccountViewModel>(accountId), Times.Once);
        }
    }
}
