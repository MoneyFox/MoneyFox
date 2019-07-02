using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditAccountViewModelTests
    {
        [Fact]
        public async Task Initialize_AccountLoaded()
        {
            // Arrange
            const int accountId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();
            crudServiceMock.Setup(x => x.ReadSingleAsync<AccountViewModel>(It.IsAny<int>()))
                           .ReturnsAsync(new AccountViewModel());

            var editAccountVm = new EditAccountViewModel(crudServiceMock.Object, null, null, null, null);

            // Act
            editAccountVm.AccountId = accountId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            crudServiceMock.Verify(x => x.ReadSingleAsync<AccountViewModel>(accountId), Times.Once);
        }
    }
}
